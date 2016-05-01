using Spring.Context.Support;

namespace Anonet.Core
{
    class MyselfNetworkPeer
    {
        private static INetworkPeer _Instance = null;

        public static INetworkPeer Instance
        {
            get
            {
                if (_Instance == null)
                {
                    var context = new XmlApplicationContext("myself.xml");
                    _Instance = context.GetObject<INetworkPeer>("peer");
                }

                return _Instance;
            }
        }
    }
}