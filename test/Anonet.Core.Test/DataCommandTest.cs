using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace Anonet.Core.Test
{
    [TestClass]
    public class DataCommandTest
    {
        private const string ErrorFormat = "DataCommand Error.";

        [TestMethod]
        public void CreateRequest()
        {
            //var entity = new PeerEntity(Guid.NewGuid().ToByteArray(), "entityName");
            //entity.IPEndPoints.Add(new IPEndPoint(IPAddress.Parse("192.168.1.100"), 1000));
            //entity.IPEndPoints.Add(new IPEndPoint(IPAddress.Parse("12.11.22.33"), 1000));

            //var dataCommand = new HeartbeatDataCommandRequest(entity);

            //var datagram = DatagramFactory.CreateRequest(dataCommand);

            //var anotherDataCommand = DatagramFactory.GetDataCommand(datagram.ConvertToBytes()) as HeartbeatDataCommandRequest;

            //var anotherEntity = anotherDataCommand.TypedEntity;

            //Assert.IsTrue(Utility.ByteArrayEqual(entity.Id, anotherEntity.Id), ErrorFormat, "byte[]");
            //Assert.AreEqual(entity.Name, anotherEntity.Name, ErrorFormat, "string");
            //Assert.IsNotNull(anotherEntity.IPEndPoints, ErrorFormat, "List<>");
            //Assert.AreEqual(entity.IPEndPoints[0], anotherEntity.IPEndPoints[0], ErrorFormat, "IPEndPoint");
            //Assert.AreEqual(entity.IPEndPoints[1], anotherEntity.IPEndPoints[1], ErrorFormat, "IPEndPoint");
        }
    }
}
