using System;
using System.Threading;

namespace RBX_Alt_Manager.Classes
{
    internal static class RobloxMultiInstanceManager
    {
        private const string CurrentSingletonName = "ROBLOX_SingletonEvent";
        private static Mutex singletonMutex;

        public static bool SetEnabled(bool enabled, out string error)
        {
            error = null;
            if (!enabled)
            {
                singletonMutex?.Dispose();
                singletonMutex = null;
                return true;
            }
            if (singletonMutex != null) return true;

            try
            {
                bool createdNew;
                singletonMutex = new Mutex(false, CurrentSingletonName, out createdNew);
                if (!createdNew)
                {
                    singletonMutex.Dispose();
                    singletonMutex = null;
                    error = "Roblox singleton mutex already exists. Close Roblox before enabling multi-instance mode.";
                    Program.Logger.Error("[MULTI] ROBLOX_SingletonEvent already exists");
                    return false;
                }
                Program.Logger.Info("[MULTI] Multi-instance mode enabled using ROBLOX_SingletonEvent mutex");
                return true;
            }
            catch (Exception exception)
            {
                singletonMutex?.Dispose();
                singletonMutex = null;
                error = "Could not create Roblox singleton mutex: " + exception.Message;
                Program.Logger.Error("[ERROR] " + error);
                return false;
            }
        }
    }
}
