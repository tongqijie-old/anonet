using System;

namespace Anonet.Core
{
    abstract class DataCommandBase<T> : IDataCommand where T : class
    {
        public object PayloadObject { get; set; }

        public virtual DataCommandIdentity Id { get; }

        public uint SerialNumber { get; set; }

        public T PayloadDataEntity { get { return PayloadObject as T; } }
    }
}