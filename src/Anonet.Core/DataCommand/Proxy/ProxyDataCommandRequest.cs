namespace Anonet.Core
{
    [DataCommand(DataCommandIdentity.Proxy, typeof(ProxyPayloadDataEntity))]
    class ProxyDataCommandRequest : DataCommandBase<ProxyPayloadDataEntity>, IDataCommandRequest
    {
        public ProxyDataCommandRequest()
        {
        }

        public ProxyDataCommandRequest(ProxyPayloadDataEntity payload)
        {
            PayloadObject = payload;
        }

        public override DataCommandIdentity Id { get { return DataCommandIdentity.Proxy; } }

        public bool NeedResponse { get { return true; } }
    }
}
