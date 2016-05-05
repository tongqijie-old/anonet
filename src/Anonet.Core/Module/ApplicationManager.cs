using Spring.Context.Support;
using System;
using System.Collections.Generic;

namespace Anonet.Core
{
    class ApplicationManager : ModuleManagerBase
    {
        private List<IModuleManager> _ModuleManagers = new List<IModuleManager>();

        public ApplicationManager()
        {
            try
            {
                _ModuleManagers.Add(ApplicationContext.GetObject<IModuleManager>("networkPeerManager"));
            }
            catch (Exception)
            {
            }
        }

        private static XmlApplicationContext ApplicationContext = new XmlApplicationContext("application.xml");

        public override void Start()
        {
            if (IsAlive)
            {
                return;
            }

            IsAlive = true;

            foreach (var moduleManager in _ModuleManagers)
            {
                moduleManager.Start();
            }
        }

        public override void Stop()
        {
            if (!IsAlive)
            {
                return;
            }

            IsAlive = false;

            foreach (var moduleManager in _ModuleManagers)
            {
                moduleManager.Stop();
            }
        }

        public override ITerminalCommandChannel[] InnerTerminalCommandChannels { get { return _ModuleManagers.ToArray(); } }
    }
}
