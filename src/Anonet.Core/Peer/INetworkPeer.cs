namespace Anonet.Core
{
    interface INetworkPeer
    {
        NetworkPeerIdentity Identity { get; }

        INetworkConnection NetworkConnection { get; }
    }
} 