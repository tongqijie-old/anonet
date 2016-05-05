using System;

namespace Anonet.Core
{
    public class App
    {
        private ApplicationManager _ApplicationManager = null;

        /// <summary>
        /// 初始化App
        /// </summary>
        public App(params object[] parameters)
        {
            _ApplicationManager = new ApplicationManager();
            _ApplicationManager.TerminalCommandExecuting += OnTerminalCommandExecuting;
            _ApplicationManager.TerminalCommandDidExecuted += OnTerminalCommandDidExecuted;
        }

        /// <summary>
        /// 执行终端命令
        /// </summary>
        public void Execute(string commandText)
        {
            var terminalCommand = TerminalCommandFactory.Create(commandText);
            if (terminalCommand == null)
            {
                Console.WriteLine("Command [{0}] cannot be parsed.", commandText);
            }

            _ApplicationManager.Consume(terminalCommand);
        }

        private void OnTerminalCommandExecuting(ITerminalCommandChannel terminalCommandChannel, ITerminalCommand terminalCommand, string prompt)
        {
            Console.WriteLine(prompt);
        }

        private void OnTerminalCommandDidExecuted(ITerminalCommandChannel terminalCommandChannel, ITerminalCommand terminalCommand)
        {
            if (terminalCommand.Result == null)
            {
                Console.WriteLine("Command [{0}] cannot be hit.", terminalCommand.TerminalCommandLine.CommandCode);
            }
            else
            {
                Console.WriteLine(terminalCommand.Result.ToString());
            }
        }
    }
}