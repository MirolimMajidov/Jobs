using FluentAssertions;
using IdentityService.Controllers;
using IdentityService.Models;
using Jobs.Service.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.UnitTests
{
    [TestClass]
    public class UserControllerTests : BaseTestEntity<User, UserController>
    {
        [TestMethod]
        public async Task GetAllCategories_ResponseDataShouldContainAllCreatedEntities()
        {
            List<User> entities = GetTestEntities();
            IEnumerable<UserDTO> dtoEntities = entities.Select(u => new UserDTO { Id = u.Id, Name = u.Name, Login = u.Login, Role = u.Role });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<UserDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = await controller.GetAll();
            response.Should().NotBeNull();

            var entitiesFromServer = response.Result as IEnumerable<UserDTO>;
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Should().HaveCount(dtoEntities.Count());
            entitiesFromServer.First().Id.Should().Be(dtoEntities.First().Id);
        }

        [TestMethod]
        public async Task GetUserById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = await controller.Get(Guid.NewGuid());
            response.ErrorId.Should().Be(404);
        }

        [TestMethod]
        public async Task GetUserById_ResponseDataShouldNotBeNull()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new UserDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Login = serverEntity.Login, Role = serverEntity.Role };

            mockRepository.Setup(p => p.GetEntityByID(serverEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<UserDTO>(serverEntity)).Returns(clientEntity);

            var response = await controller.Get(serverEntity.Id);
            response.ErrorId.Should().Be(0);

            var entityFromServer = response.Result as UserDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [TestMethod]
        public async Task CreateUser_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = await controller.Create(null);
            response.ErrorId.Should().Be(400);
        }

        [TestMethod]
        public async Task CreateUser_ThereShouldBeErrorMessage_BecausePasswordIsEmpty()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new UserDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Login = serverEntity.Login, Role = serverEntity.Role };

            var response = await controller.Create(clientEntity);
            response.ErrorId.Should().Be(400);
        }

        [TestMethod]
        public async Task CreateUser_EntityShouldBeCreatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new UserDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Login = serverEntity.Login, Role = serverEntity.Role, Password = "test123" };

            mockRepository.Setup(p => p.InsertEntity(serverEntity, true)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<User>(clientEntity)).Returns(serverEntity);
            mockMapper.Setup(p => p.Map<UserDTO>(serverEntity)).Returns(clientEntity);

            var response = await controller.Create(clientEntity);
            response.ErrorId.Should().Be(0);

            var entityFromServer = response.Result as UserDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [TestMethod]
        public async Task UpdateUser_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = await controller.Update(null);
            response.ErrorId.Should().Be(400);
        }

        [TestMethod]
        public async Task UpdateUser_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new UserDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Login = serverEntity.Login, Role = serverEntity.Role };

            User entity = null;
            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(entity);

            var response = await controller.Update(clientEntity);
            response.ErrorId.Should().Be(404);
        }

        [TestMethod]
        public async Task UpdateUser_EntityShouldBeUpdatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new UserDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Login = serverEntity.Login, Role = serverEntity.Role };

            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map(clientEntity, serverEntity)).Returns(serverEntity);
            mockRepository.Setup(p => p.UpdateEntity(serverEntity, true));

            var response = await controller.Update(clientEntity);
            response.ErrorId.Should().Be(0);
        }

        [TestMethod]
        public async Task DeleteUserById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = await controller.Delete(Guid.NewGuid());
            response.ErrorId.Should().Be(404);
        }

        [TestMethod]
        public async Task DeleteUserById_EntityShouldBeDeletedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var entityId = Guid.NewGuid();
            mockRepository.Setup(p => p.GetEntityByID(entityId)).ReturnsAsync(serverEntity);
            mockRepository.Setup(p => p.DeleteEntity(entityId, true));

            var response = await controller.Delete(entityId);
            response.ErrorId.Should().Be(0);
        }

        List<User> GetTestEntities()
        {
            return new List<User>()
                        {
                            new User { Name = "SuperAdmin", Login = "superadmin@jobs.com", Role = UserRole.SuperAdmin },
                            new User { Name = "Admin", Login = "admin@jobs.com", Role = UserRole.Admin },
                            new User { Name = "Editor", Login = "Editor@jobs.com", Role = UserRole.Editor },
                            new User { Name = "User", Login = "user@jobs.com", Role = UserRole.Admin },
                        };
        }
    }
}
