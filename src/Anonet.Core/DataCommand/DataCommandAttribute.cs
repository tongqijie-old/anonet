using System;

namespace Anonet.Core
{
    class DataCommandAttribute : Attribute
    {
        public DataCommandIdentity DataCommandIdentity { get; set; }

        public Type EntityType { get; set; }

        public DataCommandAttribute(DataCommandIdentity dataCommandIdentity)
        {
            DataCommandIdentity = dataCommandIdentity;
        }

        public DataCommandAttribute(DataCommandIdentity dataCommandIdentity, Type entityType) : this(dataCommandIdentity)
        {
            EntityType = entityType;
        }
    }
}
