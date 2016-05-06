using System.Collections.Generic;

namespace Anonet.Core
{
    class DataEntityCollection<T> : IDataEntity where T : IDataEntity
    {
        [BinEncoder("E")]
        public List<T> DataEntities { get; set; }

        [BinEncoder("C")]
        public int CurrentPage { get; set; }

        [BinEncoder("T")]
        public int TotalPages { get; set; }
    }
}
