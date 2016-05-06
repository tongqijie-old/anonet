using System;

namespace Anonet.Core
{
    [TerminalCommand(new string[] { "peeradd" })]
    class PeerAddTerminalCommand : ITerminalCommand
    {
        public PeerAddTerminalCommand(TerminalCommandLine terminalCommandLine)
        {
            TerminalCommandLine = terminalCommandLine;
        }

        public TerminalCommandLine TerminalCommandLine { get; set; }

        public bool Wait { get; set; }

        public object Result { get; set; }

        public bool Handled { get; private set; }

        public void Execute(ITerminalCommandChannel terminalCommandChannel, Action<string> prompt)
        {
            if (terminalCommandChannel is NetworkPeerManager)
            {
                Handled = true;

                var networkPeerManager = terminalCommandChannel as NetworkPeerManager;

                var networkPeerType = TerminalCommandLine["t"];
                if (networkPeerType == null 
                    || networkPeerType.Equals("normal", StringComparison.OrdinalIgnoreCase) 
                    || networkPeerType.Equals("track", StringComparison.OrdinalIgnoreCase))
                {
                    Result = TerminalCommandResult.InvalidArguments();
                    return;
                }

                var ipEndPoint = TerminalCommandLine["ep"];
                if (ipEndPoint == null)
                {
                    
                }

                // TODO

                
            }
        }
    }
}
