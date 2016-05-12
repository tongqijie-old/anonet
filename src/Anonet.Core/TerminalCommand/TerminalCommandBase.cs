namespace Anonet.Core
{
    abstract class TerminalCommandBase : ITerminalCommand
    {
        public TerminalCommandBase(TerminalCommandLine terminalCommandLine)
        {
            TerminalCommandLine = terminalCommandLine;
        }

        public TerminalCommandLine TerminalCommandLine { get; private set; }

        public bool Wait { get; set; }

        public object Result { get; protected set; }

        public bool Handled { get; protected set; }

        public virtual void Execute(ITerminalCommandChannel terminalCommandChannel, System.Action<string> prompt)
        {
        }
    }
}
