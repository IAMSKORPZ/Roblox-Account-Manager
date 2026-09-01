using System;
using System.Collections.Generic;
using System.Threading;

namespace RBX_Alt_Manager.Classes
{
    internal static class RobloxMultiInstanceManager
    {
        private static readonly string[] SingletonNames =
        {
            "ROBLOX_singletonMutex",
            "ROBLOX_SingletonEvent",
            "ROBLOX_singletonEvent"
        };
        private static readonly List<Mutex> SingletonMutexes = new List<Mutex>();

        public static bool SetEnabled(bool enabled, out string error)
        {
            error = null;
            if (!enabled)
            {
                DisposeMutexes();
                return true;
            }
            if (SingletonMutexes.Count > 0) return true;

            try
            {
                foreach (string singletonName in SingletonNames)
                {
                    bool createdNew;
                    Mutex singletonMutex = new Mutex(false, singletonName, out createdNew);
                    if (!createdNew)
                    {
                        singletonMutex.Dispose();
                        DisposeMutexes();
                        error = "Roblox singleton mutex already exists. Close Roblox before enabling multi-instance mode.";
                        Program.Logger.Error("[MULTI] " + singletonName + " already exists");
                        return false;
                    }
                    SingletonMutexes.Add(singletonMutex);
                }
                Program.Logger.Info("[MULTI] Multi-instance mode enabled using current and compatibility mutexes");
                return true;
            }
            catch (Exception exception)
            {
                DisposeMutexes();
                error = "Could not create Roblox singleton mutex: " + exception.Message;
                Program.Logger.Error("[ERROR] " + error);
                return false;
            }
        }

        private static void DisposeMutexes()
        {
            foreach (Mutex singletonMutex in SingletonMutexes)
                singletonMutex.Dispose();
            SingletonMutexes.Clear();
        }
    }
}
