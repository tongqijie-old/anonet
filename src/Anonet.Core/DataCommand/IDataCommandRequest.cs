namespace Anonet.Core
{
    interface IDataCommandRequest : IDataCommand
    {
        bool NeedResponse { get; }
    }
}
