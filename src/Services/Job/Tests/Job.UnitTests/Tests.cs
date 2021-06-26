using NUnit.Framework;

namespace Job.UnitTests
{
    [TestFixture]
    public class Tests : BaseTestEntity
    {
        [Test]
        public void MyTest()
        {
            Assert.AreEqual(1, 1, "Test");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void MultiTests(int number)
        {
            Assert.AreEqual(0, number % 2, $"{number} - number");
        }
    }
}
