using System;

namespace Anonet.Core
{
    abstract class DataCommandBase : IDataCommand
    {
        public virtual DataCommandIdentity Id { get; set; }

        public object PayloadObject { get; set; }

        public uint SerialNumber { get; set; }
    }
}
