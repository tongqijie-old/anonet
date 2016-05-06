using System;

namespace Anonet.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    class BinEncoderAttribute : Attribute
    {
        public BinEncoderAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public BinEncoderAttribute(bool nonSerialized)
        {
            NonEncode = nonSerialized;
        }

        public string PropertyName { get; set; }

        public bool NonEncode { get; set; }
    }
}
