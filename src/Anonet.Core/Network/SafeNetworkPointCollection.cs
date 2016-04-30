using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Anonet.Core
{
    class SafeNetworkPointCollection : IEnumerable<INetworkPoint>
    {
        private List<INetworkPoint> _NetworkPoints = new List<INetworkPoint>();

        public INetworkPoint[] GetAll()
        {
            return _NetworkPoints.ToArray();
        }

        public INetworkPoint GetOrAdd(INetworkPoint networkPoint)
        {
            if (!Exists(networkPoint))
            {
                Add(networkPoint);
            }

            return _NetworkPoints.FirstOrDefault(x => x.IPEndPoint.Equals(networkPoint.IPEndPoint));
        }

        private object _SyncLocker = new object();

        public void Add(INetworkPoint networkPoint)
        {
            if (!Exists(networkPoint))
            {
                lock (_SyncLocker)
                {
                    if (!Exists(networkPoint))
                    {
                        _NetworkPoints.Add(networkPoint);
                    }
                }
            }
        }

        public void AddRange(IEnumerable<INetworkPoint> networkPoints)
        {
            foreach (var networkPoint in networkPoints)
            {
                Add(networkPoint);
            }
        }

        public void Remove(INetworkPoint networkPoint)
        {
            if (Exists(networkPoint))
            {
                lock (_SyncLocker)
                {
                    if (Exists(networkPoint))
                    {
                        _NetworkPoints.Remove(_NetworkPoints.Find(x => x.Equals(networkPoint)));
                    }
                }
            }
        }

        public bool Exists(INetworkPoint networkPoint)
        {
            return _NetworkPoints.Exists(x => x.Equals(networkPoint));
        }

        public IEnumerator<INetworkPoint> GetEnumerator()
        {
            return _NetworkPoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _NetworkPoints.GetEnumerator();
        }
    }
}
