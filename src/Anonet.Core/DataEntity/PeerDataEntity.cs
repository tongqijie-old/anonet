using System.Collections.Generic;
using System.Net;

namespace Anonet.Core
{
    class PeerDataEntity : IDataEntity
    {
        public PeerDataEntity()
        {
            IPEndPoints = new List<IPEndPoint>();
        }

        public PeerDataEntity(byte[] id) : this()
        {
            Id = id;
        }

        public PeerDataEntity(byte[] id, string name) : this(id)
        {
            Name = name;
        }

        [BinarySerializable("I")]
        public byte[] Id { get; set; }

        [BinarySerializable("N")]
        public string Name { get; set; }

        [BinarySerializable("P")]
        public List<IPEndPoint> IPEndPoints { get; set; }
    }
}
