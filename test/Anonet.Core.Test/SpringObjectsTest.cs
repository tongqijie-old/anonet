using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anonet.Core.Test
{
    [TestClass]
    public class SpringObjectsTest
    {
        private const string ErrorFormat = "Spring Object Error. {0}";

        [TestMethod]
        public void ReadObject()
        {
            var myself = MyselfNetworkPeer.Instance;

            Assert.IsNotNull(myself, ErrorFormat, "myself is null.");
        }
    }
}
