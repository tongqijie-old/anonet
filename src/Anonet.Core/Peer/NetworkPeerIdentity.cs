using System;
using System.Text;

namespace Anonet.Core
{
    class NetworkPeerIdentity
    {
        public byte[] RawId { get; private set; }

        public string Nickname { get; private set; }

        public NetworkPeerIdentity(byte[] rawId)
        {
            RawId = rawId;
        }

        public NetworkPeerIdentity(byte[] rawId, string nickname) : this(rawId)
        {
            Nickname = nickname;
        }

        public NetworkPeerIdentity(string id, string nickname)
        {
            RawId = new byte[id.Length / 2];

            for (int i = 0; i < id.Length / 2; i += 2)
            {
                RawId[i / 2] = Convert.ToByte(id.Substring(i, 2), 16);
            }

            Nickname = nickname;
        }

        public string Id
        {
            get
            {
                var stringBuilder = new StringBuilder();
                foreach (var b in RawId)
                {
                    stringBuilder.Append(b.ToString("X2"));
                }
                return stringBuilder.ToString();
            }
        }

        public override bool Equals(object obj)
        {
            var identity = obj as NetworkPeerIdentity;
            if (identity == null)
            {
                return false;
            }

            return Utility.ByteArrayEqual(this.RawId, identity.RawId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Nickname ?? "unset";
        }
    }
}
