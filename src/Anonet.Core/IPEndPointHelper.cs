using System.Net;
using System.Linq;

namespace Anonet.Core
{
    static class IPEndPointHelper
    {
        public static IPEndPoint ConvertFromString(string ipEndPointString)
        {
            if (string.IsNullOrEmpty(ipEndPointString))
            {
                return null;
            }

            var addressAndPort = ipEndPointString.Split(':').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            if (addressAndPort.Length != 2)
            {
                return null;
            }

            var address = TryGetAddress(addressAndPort[0]);
            if (address == null)
            {
                return null;
            }

            var port = TryGetPort(addressAndPort[1]);
            if (port == 0)
            {
                return null;
            }

            return new IPEndPoint(address, port);
        }

        public static IPAddress TryGetAddress(string addressString)
        {
            IPAddress address;
            if (IPAddress.TryParse(addressString, out address))
            {
                return address;
            }
            else
            {
                return null;
            }
        }

        public static int TryGetPort(string portString)
        {
            int port;
            if (int.TryParse(portString, out port))
            {
                if (port > 0x0000 && port < 0xFFFF)
                {
                    return port;
                }
            }

            return 0;
        }
    }
}
