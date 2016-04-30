using System;

namespace Anonet.Core
{
    abstract class DataCommandBase : IDataCommand
    {
        public virtual DataCommandIdentity Id { get; }

        public object PayloadObject { get; set; }

        public uint SerialNumber { get; set; }
    }
}
