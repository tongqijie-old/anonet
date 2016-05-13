using System;

namespace Anonet.Core
{
    [TerminalCommand(new string[] { "appstop" })]
    class AppStopTerminalCommand : TerminalCommandBase
    {
        public AppStopTerminalCommand(TerminalCommandLine terminalCommandLine)
            : base(terminalCommandLine)
        {
        }

        public override void Execute(ITerminalCommandChannel terminalCommandChannel, Action<string> prompt)
        {
            if (terminalCommandChannel is ApplicationManager)
            {
                Handled = true;

                var applicationManager = terminalCommandChannel as ApplicationManager;

                if (TerminalCommandLine.Keys.Length == 0)
                {
                    try
                    {
                        applicationManager.Stop();

                        Result = TerminalCommandResult.Done();
                    }
                    catch (Exception e)
                    {
                        Result = TerminalCommandResult.Error(e.Message);
                    }
                }
                else
                {
                    Result = TerminalCommandResult.InvalidArguments();
                }
            }
        }
    }
}
