using System;
using System.Net;

namespace Anonet.Core
{
    class NetworkConnectionBase : INetworkConnection
    {
        public INetworkClient NetworkClient { get; private set; }

        public SafeNetworkPointCollection NetworkPoints { get; private set; }

        public INetworkPoint AvailableNetworkPoint { get; private set; }

        public DateTime StatusChangedTime { get; private set; }

        public TimeSpan StatusKeepTime { get { return DateTime.Now - StatusChangedTime; } }

        private NetworkConnectionStatus _Status = NetworkConnectionStatus.Initial;

        public NetworkConnectionStatus Status
        {
            get
            {
                return _Status;
            }
            private set
            {
                if (_Status != value)
                {
                    _Status = value;
                    StatusChangedTime = DateTime.Now;

                    if (StatusChanged != null)
                    {
                        StatusChanged.Invoke(this, _Status);
                    }
                }
            }
        }

        private object _SyncLocker = new object();

        public void UpdateStatus(NetworkConnectionStatus statusTo, params object[] parameters)
        {
            lock (_SyncLocker)
            {
                if (statusTo == NetworkConnectionStatus.None)
                {
                    switch (Status)
                    {
                        case NetworkConnectionStatus.Initial:
                            {
                                if (StatusKeepTime > GlobalConfig.Instance.TimeoutOfInitialStatus)
                                {
                                    AvailableNetworkPoint = null;
                                    Status = NetworkConnectionStatus.Pending;
                                }
                                break;
                            }
                        case NetworkConnectionStatus.Pending:
                            {
                                if (StatusKeepTime > GlobalConfig.Instance.TimeoutOfPendingStatus)
                                {
                                    AvailableNetworkPoint = null;
                                    Status = NetworkConnectionStatus.Dead;
                                }
                                break;
                            }
                        case NetworkConnectionStatus.Connected:
                            {
                                if (StatusKeepTime > GlobalConfig.Instance.TimeoutOfConnectedStatus)
                                {
                                    AvailableNetworkPoint = null;
                                    Status = NetworkConnectionStatus.Pending;
                                }
                                break;
                            }
                        case NetworkConnectionStatus.Dead:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Status = statusTo;
                    StatusChangedTime = DateTime.Now;

                    if (Status == NetworkConnectionStatus.Connected && parameters != null && parameters.Length > 0)
                    {
                        AvailableNetworkPoint = parameters[0] as INetworkPoint;
                    }
                    else
                    {
                        AvailableNetworkPoint = null;
                    }
                }
            }
        }

        public NetworkConnectionBase()
        {
            StatusChangedTime = DateTime.Now;
            NetworkPoints = new SafeNetworkPointCollection();
            
            NetworkClient = new UdpNetworkClient();
            NetworkClient.ReceivedData += OnNetworkClientReceivedData;
            NetworkClient.IsAlive = true;
        }

        private void OnNetworkClientReceivedData(byte[] data, IPEndPoint receivedFrom)
        {
            if (!Datagram.Verify(data, 0, data.Length))
            {
                return;
            }

            if (ReceivedDataCommand != null)
            {
                ReceivedDataCommand.Invoke(this, NetworkPoints.GetOrAdd(new NetworkPointBase(receivedFrom)), DatagramFactory.GetDataCommand(data));
            }
        }

        public event ConnectionReceivedDataCommandDelegate ReceivedDataCommand;

        public event ConnectionStatusChangedDelegate StatusChanged;
        
        public void Send(IDataCommand dataCommand)
        {
            if (AvailableNetworkPoint != null)
            {
                Send(dataCommand, AvailableNetworkPoint.IPEndPoint);
            }
            else
            {
                foreach (var networkPoint in NetworkPoints.GetAll())
                {
                    Send(dataCommand, networkPoint.IPEndPoint);
                }
            }
        }

        private void Send(IDataCommand dataCommand, IPEndPoint remoteEndPoint)
        {
            var payloadDataEntity = dataCommand.PayloadObject as PayloadDataEntity;
            if (payloadDataEntity != null)
            {
                payloadDataEntity.SourceId = new byte[16]; // TODO
                payloadDataEntity.TargetEndPoint = remoteEndPoint;
            }

            if (dataCommand is IDataCommandRequest)
            {
                NetworkClient.Send(DatagramFactory.CreateRequest(dataCommand as IDataCommandRequest).ConvertToBytes(), remoteEndPoint);
            }
            else if (dataCommand is IDataCommandResponse)
            {
                NetworkClient.Send(DatagramFactory.CreateResponse(dataCommand as IDataCommandResponse).ConvertToBytes(), remoteEndPoint);
            }
        }

        public void Dispose()
        {
            NetworkClient.ReceivedData -= OnNetworkClientReceivedData;
            NetworkClient.Dispose();
        }
    }
}
