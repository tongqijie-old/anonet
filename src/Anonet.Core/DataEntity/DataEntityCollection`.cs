using System.Collections.Generic;

namespace Anonet.Core
{
    class DataEntityCollection<T> : IDataEntity where T : IDataEntity
    {
        [BinEncoderElement("E")]
        public List<T> DataEntities { get; set; }

        [BinEncoderElement("C")]
        public int CurrentPage { get; set; }

        [BinEncoderElement("T")]
        public int TotalPages { get; set; }
    }
}
