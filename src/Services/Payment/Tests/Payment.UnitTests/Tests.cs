using Xunit;

namespace Payment.UnitTests
{
    public class Tests : IClassFixture<BaseTestEntity>
    {
        [Fact]
        public void MyTest()
        {
            Assert.Equal(1, 1);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void MultiTests(int number)
        {
            Assert.Equal(0, number % 2);
        }
    }
}
