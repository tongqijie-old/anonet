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

        [BinarySerializable("SI")]
        public byte[] SourceId { get; set; }

        [BinarySerializable("TEP")]
        public IPEndPoint TargetEndPoint { get; set; }
    }
}
