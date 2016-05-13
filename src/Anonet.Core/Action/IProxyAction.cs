namespace Anonet.Core
{
    interface IProxyAction : IAction
    {
        void Proxy(bool isRequest, INetworkPeer[] proxies);

        void RequestProxy(bool isRequest, INetworkPeer targetPeer);

        void TransitProxy(bool isRequest, INetworkPeer targetPeer);
    }
}
