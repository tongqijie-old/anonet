using System;

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
            return Create((byte)dataCommandRequest.Id, dataCommandRequest.NeedResponse ? (byte)(DatagramFlag.Request | DatagramFlag.NeedResponse) : (byte)DatagramFlag.Request, SerialNumber, BinEncoder.Encode(dataCommandRequest.PayloadObject));
        }

        public static Datagram Create(IDataCommandResponse dataCommandResponse)
        {
            return Create((byte)dataCommandResponse.Id, 0x00, SerialNumber, BinEncoder.Encode(dataCommandResponse.PayloadObject));
        }

        public static Datagram Create(IStreamBuffer streamBuffer)
        {
            var index = -1;
            while ((index = streamBuffer.IndexOf(Datagram.DatagramHeader)) >= 0)
            {
                streamBuffer.Seek(index);
                if (streamBuffer.Length < 13)
                {
                    return null;
                }

                var buffer = new byte[5];
                streamBuffer.ReadOnly(buffer, 0, buffer.Length);

                var length = (buffer[1] << 24) + (buffer[2] << 16) + (buffer[3] << 8) + (buffer[4]);
                if (length + 13 >= streamBuffer.Capacity)
                {
                    streamBuffer.Reset();
                    return null;
                }
                if (streamBuffer.Length < length + 13)
                {
                    return null;
                }

                var data = new byte[length + 13];
                streamBuffer.ReadOnly(data, 0, data.Length);

                if (DatagramFactory.Verify(data))
                {
                    streamBuffer.Seek(data.Length);
                    return DatagramFactory.Create(data);
                }
                else
                {
                    streamBuffer.Seek(1);
                }
            }

            return null;
        }

        public static Datagram Create(byte command, byte flag, uint serialNumber, byte[] content)
        {
            var datagram = new Datagram();

            datagram.Header = Datagram.DatagramHeader;
            datagram.Length = content != null ? (uint)content.Length : 0;
            datagram.Command = command;
            datagram.Flag = flag;
            datagram.SerialNumber = serialNumber;
            datagram.Content = content ?? new byte[0];

            return datagram;
        }

        public static Datagram Create(byte[] data)
        {
            var datagram = new Datagram();

            datagram.Header = data[0];
            datagram.Length = (uint)((data[1] << 24) + (data[2] << 16) + (data[3] << 8) + (data[4]));
            datagram.Command = data[5];
            datagram.Flag = data[6];
            datagram.SerialNumber = (uint)((data[7] << 24) + (data[8] << 16) + (data[9] << 8) + (data[10]));
            datagram.Content = new byte[datagram.Length];
            Array.Copy(data, 11, datagram.Content, 0, datagram.Content.Length);
            datagram.CheckSum = (ushort)((data[data.Length - 2] << 8) + (data[data.Length - 1]));

            return datagram;
        }

        public static bool Verify(byte[] data)
        {
            var checksum = Datagram.CalCheckSum(data, 0, data.Length - 2);
            return checksum == ((data[data.Length - 2] << 8) + data[data.Length - 1]);
        }
    }
}
