using System.Collections;
using System.Collections.Generic;

namespace Anonet.Core
{
    class SafeNetworkPeerCollection : IEnumerable<INetworkPeer>
    {
        private List<INetworkPeer> _Peers = new List<INetworkPeer>();

        private object _SyncLocker = new object();

        public void Remove(INetworkPeer removedPeer)
        {
            if (Exists(removedPeer.Identity))
            {
                lock (_SyncLocker)
                {
                    if (Exists(removedPeer.Identity))
                    {
                        _Peers.Remove(_Peers.Find(x => x.Identity == removedPeer.Identity));
                    }
                }
            }
        }

        public void RemoveAll()
        {
            lock (_SyncLocker)
            {
                _Peers.Clear();
            }
        }

        public INetworkPeer GetOrAdd(INetworkPeer peer)
        {
            if (Exists(peer.Identity))
            {
                return _Peers.Find(x => x.Identity == peer.Identity);
            }
            else
            {
                lock (_SyncLocker)
                {
                    if (!Exists(peer.Identity))
                    {
                        _Peers.Add(peer);
                    }
                }

                return peer;
            }
        }

        public bool Exists(NetworkPeerIdentity identity)
        {
            return _Peers.Exists(x => x.Identity == identity);
        }

        public INetworkPeer[] GetAll()
        {
            return _Peers.ToArray();
        }

        public IEnumerator<INetworkPeer> GetEnumerator()
        {
            return _Peers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Peers.GetEnumerator();
        }
    }
}
