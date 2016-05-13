using System.Net;
namespace Anonet.Core
{
    class ProxyPayloadDataEntity : PayloadDataEntity
    {
        public ProxyPayloadDataEntity(PeerDataEntity targetPeer)
        {
            TargetPeer = targetPeer;
        }

        [BinEncoderElement("PR")]
        public PeerDataEntity TargetPeer { get; set; }
    }
}
