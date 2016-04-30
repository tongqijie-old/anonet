namespace Anonet.Core
{
    class NetworkClientRepository
    {
        private static NetworkClientRepository _Instance = null;

        public static NetworkClientRepository Instance { get { return _Instance ?? (_Instance = new NetworkClientRepository()); } }

        public INetworkClient Create()
        {
            return new UdpNetworkClient();
        }
    }
}
