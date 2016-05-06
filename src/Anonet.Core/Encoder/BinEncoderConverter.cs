using System;
using System.Net;
using System.Text;

namespace Anonet.Core
{
    static class BinEncoderConverter
    {
        public static byte[] ConvertTo(object value)
        {
            if (value is byte)
            {
                return new byte[] { (byte)value };
            }
            else if (value is byte[])
            {
                return value as byte[];
            }
            else if (value is bool)
            {
                return new byte[] { (byte)((bool)value ? 0x01 : 0x00) };
            }
            else if (value is string)
            {
                return Encoding.UTF8.GetBytes(value as string);
            }
            else if (value is IPEndPoint)
            {
                var ep = value as IPEndPoint;
                var bytes = new byte[6];
                Array.Copy(ep.Address.GetAddressBytes(), 0, bytes, 0, 4);
                bytes[4] = (byte)(ep.Port >> 8);
                bytes[5] = (byte)ep.Port;
                return bytes;
            }
            else if (value is DateTime)
            {
                var datetime = (DateTime)value;
                return new byte[] { (byte)(datetime.Year - 1900), (byte)datetime.Month, (byte)datetime.Day, (byte)datetime.Hour, (byte)datetime.Minute, (byte)datetime.Second };
            }
            else if (value is short)
            {
                var number = (short)value;
                return new byte[] { (byte)(number >> 8), (byte)number };
            }
            else if (value is int)
            {
                var number = (int)value;
                return new byte[] { (byte)(number >> 24), (byte)(number >> 18), (byte)(number >> 8), (byte)number };
            }
            else if (value is long)
            {
                var number = (long)value;
                return new byte[] { (byte)(number >> 56), (byte)(number >> 48), (byte)(number >> 40), (byte)(number >> 32), (byte)(number >> 24), (byte)(number >> 18), (byte)(number >> 8), (byte)number };
            }
            else if (value is ushort)
            {
                var number = (ushort)value;
                return new byte[] { (byte)(number >> 8), (byte)number };
            }
            else if (value is uint)
            {
                var number = (uint)value;
                return new byte[] { (byte)(number >> 24), (byte)(number >> 18), (byte)(number >> 8), (byte)number };
            }
            else if (value is ulong)
            {
                var number = (ulong)value;
                return new byte[] { (byte)(number >> 56), (byte)(number >> 48), (byte)(number >> 40), (byte)(number >> 32), (byte)(number >> 24), (byte)(number >> 18), (byte)(number >> 8), (byte)number };
            }
            else
            {
                return null;
            }
        }

        public static object ConvertFrom(byte[] buffer, int offset, int count, Type targetType)
        {
            if (buffer == null || (offset + count) > buffer.Length) { throw new Exception(); }

            if (targetType == typeof(byte) && count == 1)
            {
                return buffer[offset];
            }
            else if (targetType == typeof(bool) && count == 1)
            {
                return buffer[offset] != 0x00;
            }
            else if (targetType == typeof(byte[]))
            {
                var array = new byte[count];
                Array.Copy(buffer, offset, array, 0, count);
                return array;
            }
            else if (targetType == typeof(string))
            {
                return Encoding.UTF8.GetString(buffer, offset, count);
            }
            else if (targetType == typeof(IPEndPoint) && count == 6)
            {
                var ip = new byte[4];
                Array.Copy(buffer, offset, ip, 0, 4);
                return new IPEndPoint(new IPAddress(ip), (buffer[offset + 4] << 8) + buffer[offset + 5]);
            }
            else if (targetType == typeof(DateTime) && count == 6)
            {
                return new DateTime(buffer[offset] + 1900, buffer[offset + 1], buffer[offset + 2], buffer[offset + 3], buffer[offset + 4], buffer[offset + 5]);
            }
            else if (targetType == typeof(short) && count == 2)
            {
                return (short)((buffer[offset] << 8) + buffer[offset + 1]);
            }
            else if (targetType == typeof(int) && count == 4)
            {
                return (buffer[offset] << 24) + (buffer[offset + 1] << 16) + (buffer[offset + 2] << 8) + buffer[offset + 3];
            }
            else if (targetType == typeof(long) && count == 8)
            {
                return (((long)buffer[offset]) << 56) + (((long)buffer[offset + 1]) << 48) + (((long)buffer[offset + 2]) << 40) + (((long)buffer[offset + 3]) << 32)
                    + (((long)buffer[offset + 4]) << 24) + (((long)buffer[offset + 5]) << 16) + (((long)buffer[offset + 6]) << 8) + buffer[offset + 7];
            }
            else if (targetType == typeof(ushort) && count == 2)
            {
                return (ushort)((buffer[offset] << 8) + buffer[offset + 1]);
            }
            else if (targetType == typeof(uint) && count == 4)
            {
                return (uint)((buffer[offset] << 24) + (buffer[offset + 1] << 16) + (buffer[offset + 2] << 8) + buffer[offset + 3]);
            }
            else if (targetType == typeof(ulong) && count == 8)
            {
                return (((ulong)buffer[offset]) << 56) + (((ulong)buffer[offset + 1]) << 48) + (((ulong)buffer[offset + 2]) << 40) + (((ulong)buffer[offset + 3]) << 32)
                    + (((ulong)buffer[offset + 4]) << 24) + (((ulong)buffer[offset + 5]) << 16) + (((ulong)buffer[offset + 6]) << 8) + buffer[offset + 7];
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
