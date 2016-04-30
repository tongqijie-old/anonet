using System.Linq;
using System.Threading.Tasks;

namespace Anonet.Core
{
    class NetworkPeerManager
    {
        private SafeNetworkPeerCollection _Peers = null;

        public SafeNetworkPeerCollection Peers { get { return _Peers ?? (_Peers = new SafeNetworkPeerCollection()); } }
        
        public bool IsAlive { get; private set; }

        private bool _IsStopped = true;

        public void Start()
        {
            if (IsAlive)
            {
                return;
            }

            IsAlive = true;

            StartAsync();
        }

        public void Stop()
        {
            if (!IsAlive)
            {
                return;
            }

            IsAlive = false;

            StopAsync();
        }

        private async void StartAsync()
        {
            _IsStopped = false;

            while (IsAlive)
            {
                var peers = Peers.GetAll();

                foreach (var peer in peers.OfType<INormalNetworkPeer>())
                {
                    peer.NetworkConnection.UpdateStatus(NetworkConnectionStatus.None);
                }

                foreach (var peer in peers.OfType<INormalNetworkPeer>().Where(x => x.NetworkConnection.Status == NetworkConnectionStatus.Connected || x.NetworkConnection.Status == NetworkConnectionStatus.Initial))
                {
                    peer.Heartbeat(true);
                }

                foreach (var peer in peers.OfType<INormalNetworkPeer>().Where(x => x.NetworkConnection.Status == NetworkConnectionStatus.Pending))
                {
                    peer.Proxy(peers.Where(x => x is ITrackNetworkPeer).ToArray());
                }

                foreach (var peer in peers.OfType<INormalNetworkPeer>().Where(x => x.NetworkConnection.Status == NetworkConnectionStatus.Dead))
                {
                    _Peers.Remove(peer);
                }

                await Task.Delay(GlobalConfig.Instance.PeriodOfPeerSync);
            }

            _IsStopped = true;
        }

        private async void StopAsync()
        {
            while (!_IsStopped)
            {
                await Task.Delay(1000);
            }

            foreach (var peer in _Peers.ToArray())
            {
                peer.NetworkConnection.Dispose();
            }

            _Peers.RemoveAll();
        }
    }
}
