namespace Anonet.Core
{
    public interface IModuleManager
    {
        void Start();

        void Stop();

        bool IsAlive { get; }
    }
}
