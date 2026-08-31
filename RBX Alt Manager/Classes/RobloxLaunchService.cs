using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RBX_Alt_Manager.Classes
{
    internal sealed class RobloxLaunchRequest
    {
        public long PlaceId { get; set; }
        public string JobId { get; set; }
        public long? TargetUserId { get; set; }
        public string PrivateServerAccessCode { get; set; }
        public string LinkCode { get; set; }
        public string AuthenticationTicket { get; set; }
        public string BrowserTrackerId { get; set; }
        public bool IsTeleport { get; set; }
    }

    internal sealed class RobloxLaunchResult
    {
        public bool Success { get; set; }
        public Process Process { get; set; }
        public string Error { get; set; }
    }

    internal static class RobloxLaunchService
    {
        private const string ProtocolName = "roblox-player";
        private static readonly TimeSpan ProcessTimeout = TimeSpan.FromSeconds(45);

        public static async Task<RobloxLaunchResult> LaunchAsync(RobloxLaunchRequest request)
        {
            if (request == null) return Failure("Launch request was empty.");
            if (string.IsNullOrWhiteSpace(request.AuthenticationTicket)) return Failure("Authentication ticket was empty.");

            string handler;
            if (!TryGetProtocolHandler(out handler))
                return Failure("Roblox protocol handler was not found. Install or repair current Roblox Windows player and try again.");

            HashSet<int> existingProcessIds = new HashSet<int>(Process.GetProcessesByName("RobloxPlayerBeta").Select(process => process.Id));
            string launchUri = BuildLaunchUri(request);

            try
            {
                Program.Logger.Info($"[LAUNCH] Launching {(request.TargetUserId.HasValue ? "follow user " + request.TargetUserId.Value : "place " + request.PlaceId)}");
                if (!string.IsNullOrWhiteSpace(request.JobId)) Program.Logger.Info("[LAUNCH] JobId supplied");
                if (!string.IsNullOrWhiteSpace(request.LinkCode)) Program.Logger.Info("[LAUNCH] Private server link code supplied");

                Process.Start(new ProcessStartInfo(launchUri) { UseShellExecute = true });
            }
            catch (Exception exception)
            {
                Program.Logger.Error($"[ERROR] Roblox protocol launch failed: {exception.Message}");
                return Failure("Windows could not invoke the Roblox protocol handler: " + exception.Message);
            }

            Program.Logger.Info("[LAUNCH] Waiting for RobloxPlayerBeta process");
            Process process = await WaitForNewPlayerProcessAsync(existingProcessIds, ProcessTimeout).ConfigureAwait(false);
            if (process == null)
            {
                Program.Logger.Error("[ERROR] RobloxPlayerBeta did not appear before launch timeout");
                return Failure("Roblox protocol handler started, but no new RobloxPlayerBeta process appeared within 45 seconds.");
            }

            Program.Logger.Info($"[LAUNCH] Roblox process detected: PID {process.Id}");
            return new RobloxLaunchResult { Success = true, Process = process };
        }

        internal static string BuildLaunchUri(RobloxLaunchRequest request)
        {
            string placeLauncherUrl;
            if (request.TargetUserId.HasValue)
            {
                placeLauncherUrl = $"https://assetgame.roblox.com/game/PlaceLauncher.ashx?request=RequestFollowUser&userId={request.TargetUserId.Value}";
            }
            else if (!string.IsNullOrWhiteSpace(request.PrivateServerAccessCode) || !string.IsNullOrWhiteSpace(request.LinkCode))
            {
                placeLauncherUrl = $"https://assetgame.roblox.com/game/PlaceLauncher.ashx?request=RequestPrivateGame&placeId={request.PlaceId}";
                if (!string.IsNullOrWhiteSpace(request.PrivateServerAccessCode)) placeLauncherUrl += "&accessCode=" + HttpUtility.UrlEncode(request.PrivateServerAccessCode);
                if (!string.IsNullOrWhiteSpace(request.LinkCode)) placeLauncherUrl += "&linkCode=" + HttpUtility.UrlEncode(request.LinkCode);
            }
            else
            {
                bool hasJobId = !string.IsNullOrWhiteSpace(request.JobId);
                placeLauncherUrl = $"https://assetgame.roblox.com/game/PlaceLauncher.ashx?request=RequestGame{(hasJobId ? "Job" : "")}&browserTrackerId={request.BrowserTrackerId}&placeId={request.PlaceId}";
                if (hasJobId) placeLauncherUrl += "&gameId=" + HttpUtility.UrlEncode(request.JobId);
                placeLauncherUrl += "&isPlayTogetherGame=false";
                if (request.IsTeleport) placeLauncherUrl += "&isTeleport=true";
            }

            long launchTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return $"roblox-player:1+launchmode:play+gameinfo:{request.AuthenticationTicket}+launchtime:{launchTime}+placelauncherurl:{HttpUtility.UrlEncode(placeLauncherUrl)}+browsertrackerid:{request.BrowserTrackerId}+robloxLocale:en_us+gameLocale:en_us+channel:+LaunchExp:InApp";
        }

        private static async Task<Process> WaitForNewPlayerProcessAsync(HashSet<int> existingProcessIds, TimeSpan timeout)
        {
            DateTime deadline = DateTime.UtcNow.Add(timeout);
            while (DateTime.UtcNow < deadline)
            {
                foreach (Process process in Process.GetProcessesByName("RobloxPlayerBeta"))
                    if (!existingProcessIds.Contains(process.Id)) return process;
                await Task.Delay(250).ConfigureAwait(false);
            }
            return null;
        }

        internal static bool TryGetProtocolHandler(out string executablePath)
        {
            executablePath = null;
            using (RegistryKey command = Registry.ClassesRoot.OpenSubKey(@"roblox-player\shell\open\command"))
            {
                string value = command?.GetValue(null) as string;
                string candidate = string.IsNullOrWhiteSpace(value) ? null : ExtractExecutablePath(value);
                if (File.Exists(candidate)) { executablePath = candidate; return true; }
            }
            foreach (RegistryKey root in new[] { Registry.CurrentUser, Registry.LocalMachine })
            {
                using (RegistryKey command = root.OpenSubKey(@"Software\Classes\roblox-player\shell\open\command"))
                {
                    string value = command?.GetValue(null) as string;
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    string candidate = ExtractExecutablePath(value);
                    if (File.Exists(candidate)) { executablePath = candidate; return true; }
                }
            }
            return false;
        }

        internal static bool TryFindPlayerExecutable(out string executablePath)
        {
            executablePath = null;
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string versionsPath = Path.Combine(localAppData, "Roblox", "Versions");
            if (!Directory.Exists(versionsPath)) return false;
            FileInfo newest = new DirectoryInfo(versionsPath).EnumerateFiles("RobloxPlayerBeta.exe", SearchOption.AllDirectories)
                .OrderByDescending(file => file.LastWriteTimeUtc).FirstOrDefault();
            if (newest == null) return false;
            executablePath = newest.FullName;
            return true;
        }

        private static string ExtractExecutablePath(string command)
        {
            command = Environment.ExpandEnvironmentVariables(command.Trim());
            if (command.StartsWith("\""))
            {
                int closingQuote = command.IndexOf('"', 1);
                return closingQuote > 1 ? command.Substring(1, closingQuote - 1) : string.Empty;
            }
            int firstSpace = command.IndexOf(' ');
            return firstSpace > 0 ? command.Substring(0, firstSpace) : command;
        }

        private static RobloxLaunchResult Failure(string error) => new RobloxLaunchResult { Success = false, Error = error };
    }
}
