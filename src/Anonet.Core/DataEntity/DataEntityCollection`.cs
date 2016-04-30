using System.Collections.Generic;

namespace Anonet.Core
{
    class DataEntityCollection<T> : IDataEntity where T : IDataEntity
    {
        [BinarySerializable("E")]
        public List<T> DataEntities { get; set; }

        [BinarySerializable("C")]
        public int CurrentPage { get; set; }

        [BinarySerializable("T")]
        public int TotalPages { get; set; }
    }
}
