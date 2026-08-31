using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RBX_Alt_Manager.Classes
{
    public static class AutoUpdaterClient
    {
        public const string GitHubRepo = "IAMSKORPZ/Roblox-Account-Manager";
        public const string LatestReleaseUrl = "https://api.github.com/repos/IAMSKORPZ/Roblox-Account-Manager/releases/latest";

        public static async Task CheckForUpdatesAsync(bool isManualCheck = false, IWin32Window owner = null)
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
                client.DefaultRequestHeaders.Add("User-Agent", "Roblox-Account-Manager");

                string releasesJson = await client.GetStringAsync(LatestReleaseUrl).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(releasesJson))
                {
                    if (isManualCheck)
                    {
                        ShowMessage("Could not retrieve update information from GitHub.", MessageBoxIcon.Warning, owner);
                    }
                    return;
                }

                JObject release = JObject.Parse(releasesJson);
                string tagName = (string)release["tag_name"] ?? string.Empty;
                string releaseName = (string)release["name"] ?? tagName;

                if (!TryParseVersion(tagName, out Version remoteVersion))
                {
                    if (isManualCheck)
                    {
                        ShowMessage("Unable to parse version from release tag: " + tagName, MessageBoxIcon.Warning, owner);
                    }
                    return;
                }

                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                if (!TryParseVersion(fvi.FileVersion, out Version currentVersion))
                {
                    currentVersion = new Version(1, 0, 0, 0);
                }

                if (remoteVersion > currentVersion)
                {
                    string downloadUrl = null;
                    JArray assets = release["assets"] as JArray;
                    if (assets != null)
                    {
                        foreach (JObject asset in assets)
                        {
                            string name = (string)asset["name"] ?? string.Empty;
                            if (name.StartsWith("Roblox.Account.Manager.", StringComparison.OrdinalIgnoreCase) &&
                                name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                            {
                                downloadUrl = (string)asset["browser_download_url"];
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(downloadUrl))
                        {
                            foreach (JObject asset in assets)
                            {
                                string name = (string)asset["name"] ?? string.Empty;
                                if (name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                                {
                                    downloadUrl = (string)asset["browser_download_url"];
                                    break;
                                }
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(downloadUrl))
                    {
                        if (isManualCheck)
                        {
                            ShowMessage($"A new version ({remoteVersion}) is available, but no release archive was found.", MessageBoxIcon.Warning, owner);
                        }
                        return;
                    }

                    bool promptResult = false;
                    string promptTitle = "Roblox Account Manager Update";
                    string promptHeading = $"Update Available: {remoteVersion}";
                    string promptMessage = $"You are currently running version {currentVersion}.\n\nWould you like to download and install version {remoteVersion} now?";

                    if (owner is Control control && control.InvokeRequired)
                    {
                        control.Invoke(new Action(() =>
                        {
                            promptResult = Utilities.YesNoPrompt(promptTitle, promptHeading, promptMessage, false);
                        }));
                    }
                    else
                    {
                        promptResult = Utilities.YesNoPrompt(promptTitle, promptHeading, promptMessage, false);
                    }

                    if (promptResult)
                    {
                        await StartDownloadAndHandoffAsync(downloadUrl, remoteVersion.ToString(), owner).ConfigureAwait(false);
                    }
                }
                else if (isManualCheck)
                {
                    ShowMessage($"You are running the latest version ({currentVersion}).", MessageBoxIcon.Information, owner);
                }
            }
            catch (Exception ex)
            {
                if (isManualCheck)
                {
                    ShowMessage($"Failed to check for updates: {ex.Message}", MessageBoxIcon.Error, owner);
                }
            }
        }

        public static bool TryParseVersion(string versionStr, out Version version)
        {
            version = null;
            if (string.IsNullOrWhiteSpace(versionStr)) return false;

            versionStr = versionStr.Trim().TrimStart('v', 'V');
            return Version.TryParse(versionStr, out version);
        }

        public static async Task StartDownloadAndHandoffAsync(string downloadUrl, string newVersionStr, IWin32Window owner = null)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "RAM_Update_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            string zipPath = Path.Combine(tempDir, $"Roblox.Account.Manager.{newVersionStr}.zip");
            string installDir = AppDomain.CurrentDomain.BaseDirectory;
            string currentExe = Application.ExecutablePath;

            try
            {
                using (var client = new HttpClient { Timeout = TimeSpan.FromMinutes(15) })
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Roblox-Account-Manager");
                    using (var fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await client.DownloadAsync(downloadUrl, fileStream, null).ConfigureAwait(false);
                    }
                }

                // Verify zip is valid
                using (ZipArchive testArchive = ZipFile.OpenRead(zipPath))
                {
                    if (testArchive.Entries.Count == 0)
                    {
                        throw new InvalidDataException("Downloaded update archive is empty.");
                    }
                }

                string installedUpdater = Path.Combine(installDir, "Roblox Account Manager Updater.exe");
                string tempUpdater = Path.Combine(tempDir, "Roblox Account Manager Updater.exe");

                if (File.Exists(installedUpdater))
                {
                    File.Copy(installedUpdater, tempUpdater, true);
                }
                else
                {
                    // Extract updater from downloaded zip
                    using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name.Equals("Roblox Account Manager Updater.exe", StringComparison.OrdinalIgnoreCase))
                            {
                                entry.ExtractToFile(tempUpdater, true);
                                break;
                            }
                        }
                    }
                }

                if (!File.Exists(tempUpdater))
                {
                    throw new FileNotFoundException("Roblox Account Manager Updater executable could not be found.");
                }

                int currentPid = Process.GetCurrentProcess().Id;
                string args = $"--pid {currentPid} --dir \"{installDir.TrimEnd('\\')}\" --zip \"{zipPath}\" --exe \"{Path.GetFileName(currentExe)}\"";

                Process.Start(new ProcessStartInfo
                {
                    FileName = tempUpdater,
                    Arguments = args,
                    WorkingDirectory = tempDir
                });

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                ShowMessage($"Update download failed: {ex.Message}", MessageBoxIcon.Error, owner);
                try
                {
                    if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
                }
                catch { }
            }
        }

        private static void ShowMessage(string message, MessageBoxIcon icon, IWin32Window owner)
        {
            if (owner is Control control && control.InvokeRequired)
            {
                control.Invoke(new Action(() =>
                {
                    MessageBox.Show(owner, message, "Roblox Account Manager", MessageBoxButtons.OK, icon);
                }));
            }
            else if (owner != null)
            {
                MessageBox.Show(owner, message, "Roblox Account Manager", MessageBoxButtons.OK, icon);
            }
            else
            {
                MessageBox.Show(message, "Roblox Account Manager", MessageBoxButtons.OK, icon);
            }
        }
    }
}
