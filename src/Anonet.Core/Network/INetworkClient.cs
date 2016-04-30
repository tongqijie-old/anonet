using System;
using System.Net;

namespace Anonet.Core
{
    delegate void NetworkClientReceivedDataDelegate(byte[] data, IPEndPoint receivedFrom);

    interface INetworkClient : IDisposable
    {
        void Send(byte[] data, IPEndPoint sendTo);

        event NetworkClientReceivedDataDelegate ReceivedData;

        bool IsAlive { get; set; }
    }
}
