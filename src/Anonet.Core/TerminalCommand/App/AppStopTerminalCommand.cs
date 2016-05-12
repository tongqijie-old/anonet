﻿using System;

namespace Anonet.Core
{
    [TerminalCommand(new string[] { "appstop" })]
    class AppStopTerminalCommand : ITerminalCommand
    {
        public AppStopTerminalCommand(TerminalCommandLine terminalCommandLine)
        {
            TerminalCommandLine = terminalCommandLine;
        }

        public TerminalCommandLine TerminalCommandLine { get; private set; }

        public bool Wait { get; set; }

        public object Result { get; private set; }

        public bool Handled { get; private set; }

        public void Execute(ITerminalCommandChannel terminalCommandChannel, Action<string> prompt)
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
