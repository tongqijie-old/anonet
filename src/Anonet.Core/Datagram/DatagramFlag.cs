namespace Anonet.Core
{
    enum DatagramFlag : byte
    {
        Request = 0x80,

        NeedResponse = 0x40,
    }
}
