namespace Anonet.Core
{
    interface IDataCommand
    {
        DataCommandIdentity Id { get; }

        uint SerialNumber { get; set; }

        object PayloadObject { get; set; }
    }
}