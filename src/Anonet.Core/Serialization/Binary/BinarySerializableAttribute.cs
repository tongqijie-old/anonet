using System;

namespace Anonet.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    class BinarySerializableAttribute : Attribute
    {
        public BinarySerializableAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public BinarySerializableAttribute(bool nonSerialized)
        {
            NonSerialized = nonSerialized;
        }

        public string PropertyName { get; set; }

        public bool NonSerialized { get; set; }
    }
}
