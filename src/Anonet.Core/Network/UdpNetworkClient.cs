using System;
using System.Net;
using System.Net.Sockets;

namespace Anonet.Core
{
    class UdpNetworkClient : INetworkClient
    {
        public UdpNetworkClient()
        {
            _StreamBuffer = new StreamBuffer(1024 * 1024);
        }

        private UdpClient _Udp = null;

        public event NetworkClientReceivedDataDelegate ReceivedData;

        private bool _IsAlive = false;

        public bool IsAlive
        {
            get { return _IsAlive; }
            set
            {
                if (_IsAlive != value)
                {
                    _IsAlive = value;

                    if (_IsAlive)
                    {
                        Run();
                    }
                    else
                    {
                        Dispose();
                    }
                }
            }
        }

        public void Send(byte[] data, IPEndPoint sendTo)
        {
            if (IsAlive && _Udp != null)
            {
                _Udp.Send(data, data.Length, sendTo);  
            }
        }

        public void Run()
        {
            RunAsync();
        }

        private async void RunAsync()
        {
            if (_Udp == null)
            {
                int tryTimes = 0;
                while (tryTimes < 100)
                {
                    try
                    {
                        _Udp = new UdpClient(NetworkPortManager.Port);
                        break;
                    }
                    catch (Exception)
                    {
                        tryTimes++;
                    }
                }
            }

            _Udp.Client.ReceiveBufferSize = 1024 * 1024;
            _Udp.Client.SendBufferSize = 1024 * 1024;

            // 处理异常
            // System.Net.Sockets.SocketException:远程主机强迫关闭了一个现有的连接。
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            _Udp.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);

            while (IsAlive)
            {
                try
                {
                    var udpReceiveResult = await _Udp.ReceiveAsync();

                    if (udpReceiveResult != null && udpReceiveResult.Buffer != null)
                    {
                        if (_StreamBuffer.Length == _StreamBuffer.Capacity)
                        {
                            _StreamBuffer.Reset();
                        }

                        _StreamBuffer.Write(udpReceiveResult.Buffer, 0, udpReceiveResult.Buffer.Length);
                    }

                    if (ReceivedData != null)
                    {
                        ReceivedData.Invoke(udpReceiveResult.Buffer, udpReceiveResult.RemoteEndPoint);
                    }
                }
                catch (Exception)
                {
                    if (!IsAlive || _Udp == null)
                    {
                        break;
                    }
                }
            }
        }

        public void Dispose()
        {
            if (IsAlive)
            {
                IsAlive = false;
            }

            if (_Udp != null)
            {
                _Udp.Close();
                _Udp = null;
            }
        }

        private IStreamBuffer _StreamBuffer = null;
    }
}
