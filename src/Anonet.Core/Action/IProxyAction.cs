namespace Anonet.Core
{
    interface IProxyAction : IAction
    {
        void Proxy(INetworkPeer[] proxies);
    }
}
