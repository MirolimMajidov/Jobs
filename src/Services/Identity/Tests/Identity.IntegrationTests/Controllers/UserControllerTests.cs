using FluentAssertions;
using IdentityService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.IntegrationTests
{
    [TestClass]
    public class UserControllerTests : BaseTestEntity
    {
        private readonly string BaseApiUri = "api/user";

        [TestMethod]
        public async Task GetAllUsers_ResponseDataShouldContainAllCreatedEntities()
        {
            var httpResponse = await Client.GetAsync(BaseApiUri);
            var response = await httpResponse.GetResponseModel();
            response.Should().NotBeNull();

            var entitiesFromServer = response.GetResponseFromResult<List<UserDTO>>();
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Count().Should().Be(4);
        }

        [TestMethod]
        public async Task GetUserById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var userId = Guid.NewGuid();
            var httpResponse = await Client.GetAsync(Path.Combine(BaseApiUri, userId.ToString()));
            var response = await httpResponse.GetResponseModel();
            response.ErrorId.Should().Be(404);
        }

        [TestMethod]
        public async Task GetUserById_ResponseDataShouldNotBeNull()
        {
            //Loading entities to get correct user id
            var httpResponseToGetEntities = await Client.GetAsync(BaseApiUri);
            var responseToGetEntities = await httpResponseToGetEntities.GetResponseModel();
            var clientEntity = responseToGetEntities.GetResponseFromResult<List<UserDTO>>().First();

            //Getting user by id
            var httpResponse = await Client.GetAsync(Path.Combine(BaseApiUri, clientEntity.Id.ToString()));
            var response = await httpResponse.GetResponseModel();
            response.ErrorId.Should().Be(0);

            var entityFromServer = response.GetResponseFromResult<UserDTO>();
            entityFromServer.Login.Should().Be(clientEntity.Login);
        }
    }
}
