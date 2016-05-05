namespace Anonet.Core
{
    interface IStreamBuffer
    {
        int Capacity { get; }

        int Length { get; }

        void Write(byte[] buffer, int offset, int count);

        void Read(byte[] buffer, int offset, int count);

        int IndexOf(byte targetByte);

        void Seek(int offset);

        void Reset();
    }
}
