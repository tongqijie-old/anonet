using System.Net;

namespace Anonet.Core
{
    interface INetworkPoint
    {
        IPEndPoint IPEndPoint { get; }
    }
}
