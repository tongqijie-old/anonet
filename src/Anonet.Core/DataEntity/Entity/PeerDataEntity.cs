using System.Collections.Generic;
using System.Net;

namespace Anonet.Core
{
    class PeerDataEntity : IDataEntity
    {
        public PeerDataEntity()
        {
        }

        public PeerDataEntity(byte[] id) : this()
        {
            Id = id;
        }

        public PeerDataEntity(byte[] id, string name) : this(id)
        {
            Name = name;
        }

        public PeerDataEntity(byte[] id, string name, IPEndPoint endPoint) : this(id, name)
        {
            EndPoint = endPoint;
        }

        [BinEncoderElement("ID")]
        public byte[] Id { get; set; }

        [BinEncoderElement("NM")]
        public string Name { get; set; }

        [BinEncoderElement("EP")]
        public IPEndPoint EndPoint { get; set; }
    }
}
