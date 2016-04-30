using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Anonet.Core.Test
{
    [TestClass]
    public class BinarySerializerTest
    {
        private const string ErrorFormat = "serialization error. {0}";

        [TestMethod]
        public void Serialize()
        {
            var entity = new PeerDataEntity(Guid.NewGuid().ToByteArray(), "entityName");
            entity.IPEndPoints.Add(new IPEndPoint(IPAddress.Parse("192.168.1.100"), 1000));
            entity.IPEndPoints.Add(new IPEndPoint(IPAddress.Parse("12.11.22.33"), 1000));

            var data = BinarySerializer.Serialize(entity);
            var anotherEntity = BinarySerializer.Deserialize(data, typeof(PeerDataEntity)) as PeerDataEntity;

            Assert.IsTrue(Utility.ByteArrayEqual(entity.Id, anotherEntity.Id), ErrorFormat, "byte[]");
            Assert.AreEqual(entity.Name, anotherEntity.Name, ErrorFormat, "string");
            Assert.IsNotNull(anotherEntity.IPEndPoints, ErrorFormat, "List<>");
            Assert.AreEqual(entity.IPEndPoints[0], anotherEntity.IPEndPoints[0], ErrorFormat, "IPEndPoint");
            Assert.AreEqual(entity.IPEndPoints[1], anotherEntity.IPEndPoints[1], ErrorFormat, "IPEndPoint");
        }
    }
}
