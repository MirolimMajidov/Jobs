using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identity.UnitTests
{
    [TestClass]
    public class Tests : BaseTestEntity
    {
        [TestMethod]
        public void MyTest()
        {
            Assert.AreEqual(1, 1, "Test");
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        public void MultiTests(int number)
        {
            Assert.IsTrue(number > 0);
        }
    }
}
