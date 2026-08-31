using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Roblox_Account_Manager_Updater
{
    static class Program
    {
        private static readonly HashSet<string> ProtectedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "AccountData.json",
            "AccountData.json.backup",
            "AccountData.json.bak",
            "RAMSettings.ini",
            "RAMTheme.ini",
            "RecentGames.json",
            "Developer.json",
            "DeveloperSettings.json",
            "ClientAppSettings.json",
            "log.txt"
        };

        private static readonly HashSet<string> ProtectedFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "logs",
            "Cookies",
            "Profiles",
            "Backups",
            "AccountData"
        };

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int pid = 0;
            string installDir = null;
            string zipPath = null;
            string exeName = "Roblox Account Manager.exe";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("--pid", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    int.TryParse(args[++i], out pid);
                }
                else if (args[i].Equals("--dir", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    installDir = args[++i];
                }
                else if (args[i].Equals("--zip", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    zipPath = args[++i];
                }
                else if (args[i].Equals("--exe", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    exeName = args[++i];
                }
            }

            if (string.IsNullOrWhiteSpace(installDir))
            {
                installDir = AppDomain.CurrentDomain.BaseDirectory;
            }

            if (string.IsNullOrWhiteSpace(zipPath) || !File.Exists(zipPath))
            {
                MessageBox.Show("No valid update package was provided to the updater.", "Roblox Account Manager Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            UpdaterForm form = new UpdaterForm();
            form.Shown += async (s, e) =>
            {
                try
                {
                    await PerformUpdateAsync(form, pid, installDir, zipPath, exeName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(form, $"Update failed: {ex.Message}", "Roblox Account Manager Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            };

            Application.Run(form);
        }

        private static async Task PerformUpdateAsync(UpdaterForm form, int pid, string installDir, string zipPath, string exeName)
        {
            // Step 1: Wait for initiating process to terminate
            form.SetStatus("Waiting for Roblox Account Manager to close...", "Closing previous session...", 10);
            if (pid > 0)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        Process process = Process.GetProcessById(pid);
                        if (!process.HasExited)
                        {
                            if (!process.WaitForExit(10000))
                            {
                                process.Kill();
                                process.WaitForExit(3000);
                            }
                        }
                    }
                    catch { }
                });
            }

            // Step 2: Extract ZIP to temporary staging folder
            form.SetStatus("Extracting update package...", "Extracting files to staging directory...", 35);
            string stagingDir = Path.Combine(Path.GetTempPath(), "RAM_Staging_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(stagingDir);

            try
            {
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(zipPath, stagingDir);
                });

                // Check for single top-level nested folder
                string sourceRoot = stagingDir;
                string[] rootDirs = Directory.GetDirectories(stagingDir);
                string[] rootFiles = Directory.GetFiles(stagingDir);

                if (rootFiles.Length == 0 && rootDirs.Length == 1)
                {
                    sourceRoot = rootDirs[0];
                }

                // Step 3: Validate staged files before touching install directory
                form.SetStatus("Validating update package...", "Verifying application binaries...", 60);
                string stagedMainExe = Path.Combine(sourceRoot, "Roblox Account Manager.exe");
                if (!File.Exists(stagedMainExe))
                {
                    throw new FileNotFoundException("Validation failed: Roblox Account Manager.exe not found in update package.");
                }

                FileInfo exeInfo = new FileInfo(stagedMainExe);
                if (exeInfo.Length < 100 * 1024)
                {
                    throw new InvalidDataException("Validation failed: Downloaded executable file is corrupted or incomplete.");
                }

                // Step 4: Safely copy application files to installation directory
                form.SetStatus("Installing update...", "Replacing application files while preserving user data...", 80);
                await Task.Run(() =>
                {
                    CopyDirectoryPreservingUserData(sourceRoot, installDir);
                });

                // Step 5: Clean up staging and zip
                form.SetStatus("Finishing update...", "Cleaning up temporary files...", 95);
                try
                {
                    if (Directory.Exists(stagingDir)) Directory.Delete(stagingDir, true);
                    if (File.Exists(zipPath)) File.Delete(zipPath);
                }
                catch { }

                // Step 6: Restart RAM
                form.SetStatus("Launching updated application...", "Restarting Roblox Account Manager...", 100);
                await Task.Delay(400);

                string targetExePath = Path.Combine(installDir, Path.GetFileName(exeName));
                if (!File.Exists(targetExePath))
                {
                    targetExePath = Path.Combine(installDir, "Roblox Account Manager.exe");
                }

                if (File.Exists(targetExePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = targetExePath,
                        WorkingDirectory = installDir
                    });
                }

                Application.Exit();
            }
            finally
            {
                try
                {
                    if (Directory.Exists(stagingDir)) Directory.Delete(stagingDir, true);
                }
                catch { }
            }
        }

        private static void CopyDirectoryPreservingUserData(string sourceDir, string destinationDir)
        {
            Directory.CreateDirectory(destinationDir);

            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string fileName = Path.GetFileName(relativePath);
                string topFolder = relativePath.Contains(Path.DirectorySeparatorChar.ToString())
                    ? relativePath.Substring(0, relativePath.IndexOf(Path.DirectorySeparatorChar))
                    : string.Empty;

                // Check protection rules
                if (ProtectedFiles.Contains(fileName)) continue;
                if (fileName.EndsWith(".bak", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".backup", StringComparison.OrdinalIgnoreCase)) continue;
                if (!string.IsNullOrEmpty(topFolder) && ProtectedFolders.Contains(topFolder)) continue;

                string destFilePath = Path.Combine(destinationDir, relativePath);
                string destDir = Path.GetDirectoryName(destFilePath);
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }

                File.Copy(file, destFilePath, true);
            }
        }
    }
}
