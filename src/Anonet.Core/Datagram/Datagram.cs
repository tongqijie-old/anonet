using System;

namespace Anonet.Core
{
    class Datagram
    {
        public const byte DatagramHeader = 0xFF;

        public byte Header { get; set; }

        public uint Length { get; set; }

        public byte Command { get; set; }

        public byte Flag { get; set; }

        public uint SerialNumber { get; set; }

        public byte[] Content { get; set; }

        public ushort CheckSum { get; set; }

        public byte[] ConvertToBytes()
        {
            var data = new byte[13 + Length];

            data[0] = DatagramHeader;
            data[1] = (byte)(Length >> 24);
            data[2] = (byte)(Length >> 16);
            data[3] = (byte)(Length >> 8);
            data[4] = (byte)(Length);
            data[5] = Command;
            data[6] = Flag;
            data[7] = (byte)(SerialNumber >> 24);
            data[8] = (byte)(SerialNumber >> 16);
            data[9] = (byte)(SerialNumber >> 8);
            data[10] = (byte)(SerialNumber);
            Array.Copy(Content, 0, data, 11, Content.Length);
            CheckSum = CalCheckSum(data, 0, data.Length - 2);

            return data;
        }

        public static bool Verify(byte[] data, int offset, int count)
        {
            if (count < 13)
            {
                return false;
            }

            if (data[offset] != DatagramHeader)
            {
                return false;
            }

            var length = (uint)((data[1] << 24) + (data[2] << 16) + (data[3] << 8) + (data[4]));

            if (count != (13 + length))
            {
                return false;
            }

            var checksum = CalCheckSum(data, offset, count - 2);

            if (checksum != ((data[offset + count - 2] << 8) + data[offset + count - 1]))
            {
                return false;
            }

            return true;
        }

        public static ushort CalCheckSum(byte[] data, int offset, int count)
        {
            if (data == null || offset >= data.Length || count <= 0 || (offset + count) >= data.Length)
            {
                return 0;
            }

            long sum = 0;
            for (var i = offset; i < (offset + count); i++)
            {
                sum += data[i];
            }
            return (ushort)sum;
        }
    }
}
