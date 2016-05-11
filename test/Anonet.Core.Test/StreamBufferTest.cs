using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anonet.Core.Test
{
    [TestClass]
    public class StreamBufferTest
    {
        [TestMethod]
        public void IndexOfTest()
        {
            var streamBuffer = Init(5, 3, 0x01);
            streamBuffer.WriteBytes(0x02, 0x03, 0x04, 0x05);
            Assert.IsTrue(streamBuffer.IndexOf(0x01) == 0);
            Assert.IsTrue(streamBuffer.IndexOf(0x02) == 3);
            Assert.IsTrue(streamBuffer.IndexOf(0x03) == 4);
            Assert.IsTrue(streamBuffer.IndexOf(0x04) == 5);
            Assert.IsTrue(streamBuffer.IndexOf(0x05) == 6);
            streamBuffer.WriteBytes(0x06, 0x07, 0x08, 0x09);
            Assert.IsTrue(streamBuffer.IndexOf(0x06) == 7);
            Assert.IsTrue(streamBuffer.IndexOf(0x07) == 8);
            Assert.IsTrue(streamBuffer.IndexOf(0x08) == 9);
            Assert.IsTrue(streamBuffer.IndexOf(0x09) == -1);
        }

        [TestMethod]
        public void SeekTest()
        {
            var streamBuffer = Init(1, 3, 0x01);
            Assert.IsTrue(streamBuffer.Length == 3);
            streamBuffer.Seek(-1);
            Assert.IsTrue(streamBuffer.Length == 4);
            streamBuffer.Seek(-2);
            Assert.IsTrue(streamBuffer.Length == 6);
            streamBuffer.Seek(-4);
            Assert.IsTrue(streamBuffer.Length == 10);
            streamBuffer.Seek(-1);
            Assert.IsTrue(streamBuffer.Length == 10);
            streamBuffer.Seek(8);
            Assert.IsTrue(streamBuffer.Length == 2);
            streamBuffer.Seek(2);
            Assert.IsTrue(streamBuffer.Length == 0);
            streamBuffer.Seek(1);
            Assert.IsTrue(streamBuffer.Length == 0);
        }

        [TestMethod]
        public void ReadTest()
        {
            var streamBuffer = Init(5, 3, 0x01);
            var buffer = new byte[5];
            streamBuffer.Read(buffer, 0, buffer.Length);
            Assert.IsTrue(buffer[0] == 0x01 && buffer[1] == 0x01 && buffer[2] == 0x01 && buffer[3] == 0x00 && buffer[4] == 0x00);
            Assert.IsTrue(streamBuffer.Length == 0);
            streamBuffer.WriteBytes(0x02, 0x02, 0x02, 0x02, 0x02, 0x03);
            buffer = new byte[5];
            streamBuffer.Read(buffer, 0, buffer.Length);
            Assert.IsTrue(buffer[0] == 0x02 && buffer[1] == 0x02 && buffer[2] == 0x02 && buffer[3] == 0x02 && buffer[4] == 0x02);
            Assert.IsTrue(streamBuffer.Length == 1);
            Assert.IsTrue(streamBuffer.ReadByte() == 0x03);
            Assert.IsTrue(streamBuffer.ReadAll().Length == 0);
        }

        [TestMethod]
        public void WriteTest()
        {
            var streamBuffer = Init(5, 0, 0x00);
            var buffer = new byte[6] { 0x01, 0x01, 0x01, 0x01, 0x01, 0x01 };
            streamBuffer.Write(buffer, 0, 4);
            Assert.IsTrue(streamBuffer.Length == 4);
            streamBuffer.Write(buffer, 4, 2);
            Assert.IsTrue(streamBuffer.Length == 6);
            streamBuffer.WriteBytes(0x02, 0x02, 0x02, 0x02, 0x02);
            Assert.IsTrue(streamBuffer.Length == 10);
        }

        private StreamBuffer Init(int start, int count, byte b)
        {
            var streamBuffer = new StreamBuffer(10);
            for (int i = 0; i < start; i++)
            {
                streamBuffer.Write(new byte[] { 0x00 });
            }
            streamBuffer.Seek(start);

            for (int i = 0; i < count; i++)
            {
                streamBuffer.Write(new byte[] { b });
            }
            
            return streamBuffer;
        }
    }
}
