namespace Anonet.Core
{
    class NormalNetworkPeerBase : INormalNetworkPeer
    {
        public NetworkPeerIdentity Identity { get; private set; }

        public INetworkConnection NetworkConnection { get; private set; }

        public NormalNetworkPeerBase(NetworkPeerIdentity identity)
        {
            Identity = identity;
            NetworkConnection = new NetworkConnectionBase();
            NetworkConnection.ReceivedDataCommand += OnReceivedDataCommand;
        }

        public NormalNetworkPeerBase(NetworkPeerIdentity identity, INetworkPoint[] networkPoints) : this(identity)
        {
            NetworkConnection.NetworkPoints.AddRange(networkPoints);
        }

        private void OnReceivedDataCommand(INetworkConnection connection, INetworkPoint networkPoint, IDataCommand dataCommand)
        {
            if (dataCommand is HeartbeatDataCommandRequest)
            {
                NetworkConnection.UpdateStatus(NetworkConnectionStatus.Connected, networkPoint);

                Heartbeat(false);
            }
            else if (dataCommand is HeartbeatDataCommandResponse)
            {
                NetworkConnection.UpdateStatus(NetworkConnectionStatus.Connected, networkPoint);
            }
        }

        public void Heartbeat(bool request)
        {
            if (request)
            {
                NetworkConnection.Send(new HeartbeatDataCommandRequest(new HeartbeatPayloadDataEntity()));
            }
            else
            {
                NetworkConnection.Send(new HeartbeatDataCommandResponse(new HeartbeatPayloadDataEntity()));
            }
        }

        public void Proxy(INetworkPeer[] proxies)
        {
            //throw new NotImplementedException();
        }
    }
}
