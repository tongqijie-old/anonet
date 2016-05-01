namespace Anonet.Core
{
    interface INetworkPeer : IKeepAliveAction
    {
        NetworkPeerIdentity Identity { get; }

        INetworkConnection NetworkConnection { get; }
    }
} 