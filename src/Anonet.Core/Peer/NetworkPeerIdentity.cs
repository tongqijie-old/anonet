using System;
using System.Text;

namespace Anonet.Core
{
    class NetworkPeerIdentity
    {
        private byte[] _RawId = null;

        private string Nickname { get; set; }

        public NetworkPeerIdentity(byte[] rawId)
        {
            if (rawId == null || rawId.Length == 0)
            {
                throw new ArgumentException("argument value cannot be null or empty.", "rawId");
            }

            _RawId = rawId;
        }

        public NetworkPeerIdentity(byte[] rawId, string nickname) : this(rawId)
        {
            Nickname = nickname;
        }

        public string Id
        {
            get
            {
                var stringBuilder = new StringBuilder();
                foreach (var b in _RawId)
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

            return Utility.ByteArrayEqual(this._RawId, identity._RawId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Nickname ?? "unset";
        }

        public static bool operator !=(NetworkPeerIdentity x, NetworkPeerIdentity y)
        {
            return !x.Equals(y);
        }

        public static bool operator ==(NetworkPeerIdentity x, NetworkPeerIdentity y)
        {
            return x.Equals(y);
        }
    }
}
