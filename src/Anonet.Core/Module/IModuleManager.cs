namespace Anonet.Core
{
    interface IModuleManager : ITerminalCommandChannel
    {
        void Start();

        void Stop();

        bool IsAlive { get; }
    }
}
