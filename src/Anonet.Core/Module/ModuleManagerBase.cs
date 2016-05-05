namespace Anonet.Core
{
    abstract class ModuleManagerBase : IModuleManager
    {
        public virtual void Start()
        {
            IsAlive = true;
        }

        public virtual void Stop()
        {
            IsAlive = false;
        }

        public bool IsAlive { get; set; }

        public void Consume(ITerminalCommand terminalCommand)
        {
            terminalCommand.Execute(this, (text) =>
            {
                if (TerminalCommandExecuting != null)
                {
                    TerminalCommandExecuting.Invoke(this, terminalCommand, text);
                }
            });

            if (terminalCommand.Handled)
            {
                if (TerminalCommandDidExecuted != null)
                {
                    TerminalCommandDidExecuted.Invoke(this, terminalCommand);
                }
            }

            foreach (var innerTerminalCommandChannel in InnerTerminalCommandChannels)
            {
                innerTerminalCommandChannel.Consume(terminalCommand);

                if (terminalCommand.Handled)
                {
                    return;
                }
            }
        }

        public event TerminalCommandExecutingDelegate TerminalCommandExecuting;

        public event TerminalCommandDidExecutedDelegate TerminalCommandDidExecuted;

        public virtual ITerminalCommandChannel[] InnerTerminalCommandChannels
        {
            get { return new ITerminalCommandChannel[0]; }
        }
    }
}
