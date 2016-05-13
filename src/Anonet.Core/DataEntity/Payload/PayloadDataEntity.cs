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

        [BinEncoderElement("SI")]
        public byte[] SourceId { get; set; }

        [BinEncoderElement("TP")]
        public IPEndPoint TargetEndPoint { get; set; }
    }
}
