using System.Net;

namespace Anonet.Core
{
    class NetworkPointBase : INetworkPoint
    {
        public IPEndPoint IPEndPoint { get; private set; }

        public NetworkPointBase(IPEndPoint ipEndPoint)
        {
            IPEndPoint = ipEndPoint;
        }

        public NetworkPointBase(string ipEndPointString)
        {
            var addressAndPort = ipEndPointString.Split(':');
            if (addressAndPort.Length == 2)
            {
                IPEndPoint = new IPEndPoint(IPAddress.Parse(addressAndPort[0]), int.Parse(addressAndPort[1]));
            }
        }

        public override bool Equals(object obj)
        {
            var networkPoint = obj as INetworkPoint;
            if (networkPoint == null)
            {
                return false;
            }

            return this.Equals(networkPoint);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
