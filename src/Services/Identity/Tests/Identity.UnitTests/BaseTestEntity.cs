using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identity.UnitTests
{
    public abstract class BaseTestEntity
    {
        [TestInitialize]
        public void Setup()
        {
            // Runs before each test. (Optional)
        }

        [TestCleanup]
        public void TearDown()
        {
            // Runs after each test. (Optional)
        }
    }
}
