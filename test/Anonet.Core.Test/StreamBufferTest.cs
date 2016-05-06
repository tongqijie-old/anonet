using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anonet.Core.Test
{
    [TestClass]
    public class StreamBufferTest
    {
        [TestMethod]
        public void IO()
        {
            var streamBuffer = new StreamBuffer(10);
            streamBuffer.Write(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 });
            Assert.IsTrue(streamBuffer.Length == 5);
            streamBuffer.Write(new byte[] { 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A });
            Assert.IsTrue(streamBuffer.Length == 10);

            var buffer = new byte[10];
            streamBuffer.Read(buffer, 0, 6);
            Assert.IsTrue(buffer[0] == 0x00 && buffer[1] == 0x01 && buffer[2] == 0x02 && buffer[3] == 0x03 && buffer[4] == 0x04 && buffer[5] == 0x05);
            Assert.IsTrue(streamBuffer.Length == 4);
            streamBuffer.Read(buffer, 0, 10);
            Assert.IsTrue(buffer[0] == 0x06 && buffer[1] == 0x07 && buffer[2] == 0x08 && buffer[3] == 0x09);
            Assert.IsTrue(streamBuffer.Length == 0);

            streamBuffer.Write(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });
            Assert.IsTrue(streamBuffer.Length == 7);
            buffer = streamBuffer.ReadAll();
            streamBuffer.Write(new byte[] { 0x07, 0x08, 0x09, 0x0A, 0x0B });
            Assert.IsTrue(streamBuffer.Length == 5);
        }
    }
}
