namespace Anonet.Core
{
    class MyselfNetworkPeer
    {
        private static INetworkPeer _Instance = null;

        public static INetworkPeer Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new NormalNetworkPeerBase(new NetworkPeerIdentity("", ""));
                }

                return _Instance;
            }
        }
    }
}