using System;
using System.Collections.Generic;

namespace Anonet.Core
{
    class ApplicationManager : ModuleManagerBase
    {
        private List<IModuleManager> _ModuleManagers = new List<IModuleManager>();

        public ApplicationManager()
        {
            _ModuleManagers.Add(new NetworkPeerManager());
            foreach (var module in _ModuleManagers)
            {
                module.TerminalCommandExecuting += OnTerminalCommandExecuting;
                module.TerminalCommandDidExecuted += OnTerminalCommandDidExecuted;
            }
        }

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

        private void OnTerminalCommandExecuting(ITerminalCommandChannel terminalCommandChannel, ITerminalCommand terminalCommand, string prompt)
        {
            FireTerminalCommandExecuting(terminalCommand, prompt);
        }

        private void OnTerminalCommandDidExecuted(ITerminalCommandChannel terminalCommandChannel, ITerminalCommand terminalCommand)
        {
            FireTerminalCommandDidExecuted(terminalCommand);
        }
    }
}
