namespace Anonet.Core
{
    class DatagramFactory
    {
        private static uint _SerialNumber = 0;

        private static uint SerialNumber
        {
            get
            {
                return ++_SerialNumber;
            }
        }

        public static Datagram Create(IDataCommandRequest dataCommandRequest)
        {
            return Datagram.Create((byte)dataCommandRequest.Id, dataCommandRequest.NeedResponse ? (byte)(DatagramFlag.Request | DatagramFlag.NeedResponse) : (byte)DatagramFlag.Request, SerialNumber, BinEncoder.Encode(dataCommandRequest.PayloadObject));
        }

        public static Datagram Create(IDataCommandResponse dataCommandResponse)
        {
            return Datagram.Create((byte)dataCommandResponse.Id, 0x00, SerialNumber, BinEncoder.Encode(dataCommandResponse.PayloadObject));
        }

        public static Datagram Create(IStreamBuffer streamBuffer)
        {
            var index = -1;
            while ((index = streamBuffer.IndexOf(Datagram.DatagramHeader)) >= 0)
            {
                // TODO
            }

            return null;
        }
    }
}
