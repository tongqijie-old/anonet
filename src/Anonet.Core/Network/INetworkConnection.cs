using System;

namespace Anonet.Core
{
    delegate void ConnectionStatusChangedDelegate(INetworkConnection connection, NetworkConnectionStatus status);

    delegate void ConnectionReceivedDataCommandDelegate(INetworkConnection connection, INetworkPoint networkPoint, IDataCommand dataCommand);

    interface INetworkConnection : IDisposable
    {
        NetworkConnectionStatus Status { get; }

        event ConnectionStatusChangedDelegate StatusChanged;

        DateTime StatusChangedTime { get; }

        SafeNetworkPointCollection NetworkPoints { get; }

        INetworkPoint AvailableNetworkPoint { get; }

        INetworkClient NetworkClient { get; }

        void Send(IDataCommand dataCommand);

        event ConnectionReceivedDataCommandDelegate ReceivedDataCommand;

        void UpdateStatus(NetworkConnectionStatus statusTo, params object[] paramters);
    }
}
