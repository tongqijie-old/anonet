using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Anonet.Core
{
    class ThreadSafeNetworkPeerCollection : IThreadSafeCollection<INetworkPeer>
    {
        private List<INetworkPeer> _Peers = new List<INetworkPeer>();

        private object _SyncLocker = new object();

        public void Remove(INetworkPeer removedPeer)
        {
            if (Exists(removedPeer))
            {
                lock (_SyncLocker)
                {
                    if (Exists(removedPeer))
                    {
                        _Peers.Remove(_Peers.Find(x => x.Identity == removedPeer.Identity));
                    }
                }
            }
        }

        public void RemoveAll()
        {
            INetworkPeer peer = null;
            while ((peer = _Peers.FirstOrDefault()) != null)
            {
                Remove(peer);
            }
        }

        public INetworkPeer GetOrAdd(INetworkPeer peer)
        {
            if (Exists(peer))
            {
                return _Peers.Find(x => x.Identity == peer.Identity);
            }
            else
            {
                Add(peer);

                return peer;
            }
        }

        public bool Exists(INetworkPeer peer)
        {
            return _Peers.Exists(x => x.Identity == peer.Identity);
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

        public void AddRange(IEnumerable<INetworkPeer> peers)
        {
            foreach (var peer in peers)
            {
                Add(peer);
            }
        }

        public void Add(INetworkPeer peer)
        {
            if (!Exists(peer))
            {
                lock (_SyncLocker)
                {
                    if (!Exists(peer))
                    {
                        _Peers.Add(peer);
                    }
                }
            }
        }
    }
}
