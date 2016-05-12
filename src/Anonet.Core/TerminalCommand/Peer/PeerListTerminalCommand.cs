using System.Text;
namespace Anonet.Core
{
    [TerminalCommand(new string[] { "peerlist", "pls" })]
    class PeerListTerminalCommand : TerminalCommandBase
    {
        public PeerListTerminalCommand(TerminalCommandLine terminalCommandLine) 
            : base(terminalCommandLine)
        {
        }

        public override void Execute(ITerminalCommandChannel terminalCommandChannel, System.Action<string> prompt)
        {
            if (terminalCommandChannel is NetworkPeerManager)
            {
                Handled = true;

                var networkPeerManager = terminalCommandChannel as NetworkPeerManager;

                foreach (var peer in networkPeerManager.Peers.GetAll())
                {
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append(peer.Identity == null ? "unset:" : peer.Identity.ToString());
                    foreach (var ep in peer.NetworkConnection.NetworkPoints)
                    {
                        if (peer.NetworkConnection.AvailableNetworkPoint == null)
                        {
                            stringBuilder.Append(string.Format("{0}({1})", ep.ToString(), "inactive"));
                        }
                        else
                        {
                            stringBuilder.Append(string.Format("{0}({1})", ep.ToString(), peer.NetworkConnection.AvailableNetworkPoint.Equals(ep) ? "active" : "inactive"));
                        }
                    }
                    prompt.Invoke(stringBuilder.ToString());
                }

                Result = TerminalCommandResult.Done();
            }
        }
    }
}
