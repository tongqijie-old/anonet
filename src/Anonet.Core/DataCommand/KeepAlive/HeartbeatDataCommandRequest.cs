using System;

namespace Anonet.Core
{
    [DataCommand(DataCommandIdentity.HeartBeat, typeof(HeartbeatPayloadDataEntity))]
    class HeartbeatDataCommandRequest : DataCommandBase<HeartbeatPayloadDataEntity>, IDataCommandRequest
    {
        public HeartbeatDataCommandRequest()
        {
        }

        public HeartbeatDataCommandRequest(HeartbeatPayloadDataEntity payload)
        {
            PayloadObject = payload;
        }

        public override DataCommandIdentity Id { get { return DataCommandIdentity.HeartBeat; } }

        public bool NeedResponse { get { return true; } }
    }
}