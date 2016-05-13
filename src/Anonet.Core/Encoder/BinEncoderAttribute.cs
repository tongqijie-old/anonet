using System;

namespace Anonet.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    class BinEncoderElementAttribute : Attribute
    {
        public BinEncoderElementAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public BinEncoderElementAttribute(bool nonSerialized)
        {
            NonEncode = nonSerialized;
        }

        public string PropertyName { get; set; }

        public bool NonEncode { get; set; }
    }
}
