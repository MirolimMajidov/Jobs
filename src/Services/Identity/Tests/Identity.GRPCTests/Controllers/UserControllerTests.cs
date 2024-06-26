using FluentAssertions;

namespace Identity.GRPCTests
{
    using Grpc.Core;
    using IdentityClient.Protos;

    public class UserControllerTests : BaseTestEntity
    {
        [Test]
        public async Task GetUsers_ReturnsUserStream()
        {
            // Arrange
            var client = new User.UserClient(_grpcChannel);

            // Act
            var users = await GetAllUsers(client);

            // Assert
            users.Count().Should().Be(4);
        }

        [Test]
        public async Task GetUserById_ReturnsUser()
        {
            // Arrange
            var client = new User.UserClient(_grpcChannel);
            var users = await GetAllUsers(client);
            var firstUserId = users.First().Id.ToString();
            var request = new UserRequest { Id = firstUserId };

            // Act
            var user = await client.GetUserByIdAsync(request);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(firstUserId);
        }

        static async Task<List<UserReply>> GetAllUsers(User.UserClient client)
        {
            var request = new UsersRequest();
            var call = client.GetUsers(request);
            var users = new List<UserReply>();

            await foreach (var user in call.ResponseStream.ReadAllAsync())
            {
                users.Add(user);
            }
            return users;
        }
    }
}
