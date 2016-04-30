namespace Anonet.Core
{
    class MyselfNetworkPeer : INetworkPeer
    {
        private static MyselfNetworkPeer _Instance = null;

        public static MyselfNetworkPeer Instance { get { return _Instance ?? (_Instance = new MyselfNetworkPeer()); } }

        public NetworkPeerIdentity Identity { get; private set; }

        public INetworkConnection NetworkConnection { get; private set; }

        private MyselfNetworkPeer()
        {
            // Load identity from local config file
            
        }
    }
}