using Spring.Context.Support;
using System;

namespace Anonet.Core
{
    class ApplicationManager : IModuleManager
    {
        private IModuleManager _NetworkPeerManager = null;

        public ApplicationManager()
        {
            try
            {
                _NetworkPeerManager = ApplicationContext.GetObject<IModuleManager>("networkPeerManager");
            }
            catch (Exception)
            {
            }
        }

        private static XmlApplicationContext ApplicationContext = new XmlApplicationContext("application.xml");

        public bool IsAlive { get; private set; }

        public void Start()
        {
            if (IsAlive)
            {
                return;
            }

            IsAlive = true;

            if (_NetworkPeerManager != null)
            {
                _NetworkPeerManager.Start();
            }
        }

        public void Stop()
        {
            if (!IsAlive)
            {
                return;
            }

            IsAlive = false;

            if (_NetworkPeerManager != null)
            {
                _NetworkPeerManager.Stop();
            }
        }
    }
}
