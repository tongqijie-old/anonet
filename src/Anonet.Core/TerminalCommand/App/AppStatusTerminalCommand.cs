namespace Anonet.Core
{
    [TerminalCommand(new string[] { "appstatus" })]
    class AppStatusTerminalCommand : TerminalCommandBase
    {
        public AppStatusTerminalCommand(TerminalCommandLine terminalCommandLine)
            : base(terminalCommandLine)
        {
        }

        public override void Execute(ITerminalCommandChannel terminalCommandChannel, System.Action<string> prompt)
        {
            if (terminalCommandChannel is ApplicationManager)
            {
                Handled = true;

                var applicationManager = terminalCommandChannel as ApplicationManager;

                prompt.Invoke(string.Format("status: {0}", applicationManager.IsAlive ? "active" : "inactive"));

                Result = TerminalCommandResult.Done();
            }
        }
    }
}