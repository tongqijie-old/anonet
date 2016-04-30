namespace Anonet.Core
{
    [DataCommand(DataCommandIdentity.HeartBeat, typeof(HeartbeatPayloadDataEntity))]
    class HeartbeatDataCommandResponse : DataCommandBase<PeerDataEntity>, IDataCommandResponse
    {
        public HeartbeatDataCommandResponse()
        {
        }

        public HeartbeatDataCommandResponse(HeartbeatPayloadDataEntity payload)
        {
            PayloadObject = payload;
        }

        public override DataCommandIdentity Id { get { return DataCommandIdentity.HeartBeat; } }
    }
}
