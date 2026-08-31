using System;
using System.Threading;

namespace RBX_Alt_Manager.Classes
{
    internal static class RobloxMultiInstanceManager
    {
        private const string CurrentSingletonName = "ROBLOX_singletonEvent";
        private static EventWaitHandle singletonEvent;

        public static bool SetEnabled(bool enabled, out string error)
        {
            error = null;
            if (!enabled)
            {
                singletonEvent?.Dispose();
                singletonEvent = null;
                return true;
            }
            if (singletonEvent != null) return true;

            try
            {
                bool createdNew;
                singletonEvent = new EventWaitHandle(false, EventResetMode.ManualReset, CurrentSingletonName, out createdNew);
                if (!createdNew)
                {
                    singletonEvent.Dispose();
                    singletonEvent = null;
                    error = "Roblox singleton event already exists. Close Roblox before enabling multi-instance mode.";
                    Program.Logger.Error("[MULTI] ROBLOX_singletonEvent already exists");
                    return false;
                }
                Program.Logger.Info("[MULTI] Multi-instance mode enabled using ROBLOX_singletonEvent");
                return true;
            }
            catch (Exception exception)
            {
                singletonEvent?.Dispose();
                singletonEvent = null;
                error = "Could not create Roblox singleton event: " + exception.Message;
                Program.Logger.Error("[ERROR] " + error);
                return false;
            }
        }
    }
}
