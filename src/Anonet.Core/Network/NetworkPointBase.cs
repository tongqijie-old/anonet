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

        public override bool Equals(object obj)
        {
            var networkPoint = obj as INetworkPoint;
            if (networkPoint == null)
            {
                return false;
            }

            return this.IPEndPoint.Equals(networkPoint.IPEndPoint);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
