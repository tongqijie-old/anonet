namespace Anonet.Core
{
    static class NetworkPortManager
    {
        private static int _Port = 40000;

        public static int Port
        {
            get
            {
                if (_Port >= 0xFFFF)
                {
                    _Port = 40000;
                }
                return _Port++;
            }
        }
    }
}
