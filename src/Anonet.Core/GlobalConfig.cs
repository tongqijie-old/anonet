using System;

namespace Anonet.Core
{
    class GlobalConfig
    {
        private static GlobalConfig _Instance = null;

        public static GlobalConfig Instance { get { return _Instance ?? (_Instance = new GlobalConfig()); } }

        private GlobalConfig()
        {
            // load from config file
            PeriodOfPeerSync = TimeSpan.FromSeconds(3);

            TimeoutOfInitialStatus = TimeSpan.FromSeconds(10);
            TimeoutOfPendingStatus = TimeSpan.FromSeconds(10);
            TimeoutOfConnectedStatus = TimeSpan.FromSeconds(20);
        }

        public TimeSpan PeriodOfPeerSync { get; set; }

        public TimeSpan TimeoutOfInitialStatus { get; set; }

        public TimeSpan TimeoutOfPendingStatus { get; set; }

        public TimeSpan TimeoutOfConnectedStatus { get; set; }
    }
}
