namespace Anonet.Core
{
    [DataCommand(DataCommandIdentity.Proxy, typeof(ProxyPayloadDataEntity))]
    class ProxyDataCommandResponse : DataCommandBase<ProxyPayloadDataEntity>, IDataCommandResponse
    {
        public ProxyDataCommandResponse()
        {
        }

        public ProxyDataCommandResponse(ProxyPayloadDataEntity payload)
        {
            PayloadObject = payload;
        }

        public override DataCommandIdentity Id { get { return DataCommandIdentity.Proxy; } }
    }
}