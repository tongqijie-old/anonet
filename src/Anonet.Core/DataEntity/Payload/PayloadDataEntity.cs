using System.Net;

namespace Anonet.Core
{
    class PayloadDataEntity : IDataEntity
    {
        public PayloadDataEntity()
        {
        }

        public PayloadDataEntity(byte[] sourceId, IPEndPoint targetEndPoint)
        {
            SourceId = sourceId;
            TargetEndPoint = targetEndPoint;
        }

        [BinEncoder("SI")]
        public byte[] SourceId { get; set; }

        [BinEncoder("TEP")]
        public IPEndPoint TargetEndPoint { get; set; }
    }
}
