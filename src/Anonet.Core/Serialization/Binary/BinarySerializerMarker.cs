namespace Anonet.Core
{
    enum BinarySerializerMarker : byte
    {
        ObjectMarker = 0xFF,

        PropertyNameMarker = 0xF1,

        PropertyValueMarker = 0xF0,
    }
}