using Exodus.API.Models;
using Exodus.Domain;
using Exodus.Exceptions;
using Exodus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace Exodus.API.Helpers
{
    public static class API_KeyHelper
    {
        private static Timer timer;
        private static double TimerInterval = 60 * 1000; // Every Minute

        private static Dictionary<string, API_Session> dicApiSesions = new Dictionary<string, API_Session>();

        private static object lockRemoving = new object();
        private static bool IsCleanNow = false;
        public static int LifeTimeDefault { get; set; } = 8; // hours

        static API_KeyHelper()
        {
            // Start timer
            timer = new Timer(TimerInterval);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public static bool CheckKey(string api_key)
        {
            lock (lockRemoving)
            {
                return dicApiSesions.ContainsKey(api_key ?? "");
            }
        }

        public static string GenarateKey(long UserID)
        {
            // Generate Key
            string key = Guid.NewGuid().ToString().Replace("-", "");
            // add to session
            lock (lockRemoving)
            {
                dicApiSesions.Add(key, new API_Session(UserID));
            }
            // Add Key to User
            return key;
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsCleanNow) { return; }
            IsCleanNow = true;
            try
            {
                lock (lockRemoving)
                {
                    foreach (string item in dicApiSesions.Where(a => !a.Value.IsValid).Select(a => a.Key).ToArray())
                    {
                        dicApiSesions.Remove(item);
                    }
                }
            }
            catch(Exception) { }
            finally { IsCleanNow = false; }
        }
    }
}