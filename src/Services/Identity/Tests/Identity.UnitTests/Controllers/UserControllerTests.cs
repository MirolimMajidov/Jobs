using FluentAssertions;
using IdentityService.Controllers;
using IdentityService.Models;
using Jobs.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Identity.UnitTests
{
    [TestClass]
    public class UserControllerTests : BaseTestEntity<User, UserController>
    {
        [TestMethod]
        public void GetAllCategories_ResponseDataShouldContainAllCreatedEntities()
        {
            List<User> entities = GetTestEntities();
            IEnumerable<UserDTO> dtoEntities = entities.Select(u => new UserDTO { Id = u.Id, Name = u.Name, Login = u.Login, Role = u.Role });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<UserDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = controller.GetAll().Result;
            response.Should().NotBeNull();

            var entitiesFromServer = response.Result as IEnumerable<UserDTO>;
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Should().HaveCount(dtoEntities.Count());
            entitiesFromServer.First().Should().Equals(dtoEntities.First());
        }

        [TestMethod]
        public void GetUserById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = controller.Get(Guid.NewGuid()).Result;
            response.ErrorId.Should().Be(404);
        }

        [TestMethod]
        public void GetUserById_ResponseDataShouldNotBeNull()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new UserDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Login = serverEntity.Login, Role = serverEntity.Role };

            mockRepository.Setup(p => p.GetEntityByID(serverEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<UserDTO>(serverEntity)).Returns(clientEntity);

            var response = controller.Get(serverEntity.Id).Result;
            response.ErrorId.Should().Be(0);
            response.Should().NotBeNull();

            var entityFromServer = response.Result as UserDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [TestMethod]
        public void CreateUser_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = controller.Create(null).Result;
            response.ErrorId.Should().Be(400);
        }

        [TestMethod]
        public void CreateUser_ThereShouldBeErrorMessage_BecausePasswordIsEmpty()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new UserDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Login = serverEntity.Login, Role = serverEntity.Role };

            var response = controller.Create(clientEntity).Result;
            response.ErrorId.Should().Be(400);
        }

        [TestMethod]
        public void CreateUser_EntityShouldBeCreatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new UserDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Login = serverEntity.Login, Role = serverEntity.Role, Password = "test123" };

            mockRepository.Setup(p => p.InsertEntity(serverEntity, true)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<User>(clientEntity)).Returns(serverEntity);
            mockMapper.Setup(p => p.Map<UserDTO>(serverEntity)).Returns(clientEntity);

            var response = controller.Create(clientEntity).Result;
            response.ErrorId.Should().Be(0);
            response.Should().NotBeNull();

            var entityFromServer = response.Result as UserDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [TestMethod]
        public void UpdateUser_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = controller.Update(null).Result;
            response.ErrorId.Should().Be(400);
        }

        [TestMethod]
        public void UpdateUser_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new UserDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Login = serverEntity.Login, Role = serverEntity.Role };

            User entity = null;
            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(entity);

            var response = controller.Update(clientEntity).Result;
            response.ErrorId.Should().Be(404);
        }

        [TestMethod]
        public void UpdateUser_EntityShouldBeUpdatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new UserDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Login = serverEntity.Login, Role = serverEntity.Role };

            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map(clientEntity, serverEntity)).Returns(serverEntity);
            mockRepository.Setup(p => p.UpdateEntity(serverEntity, true));

            var response = controller.Update(clientEntity).Result;
            response.ErrorId.Should().Be(0);
            response.Should().NotBeNull();
        }

        [TestMethod]
        public void DeleteUserById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = controller.Delete(Guid.NewGuid()).Result;
            response.ErrorId.Should().Be(404);
        }

        [TestMethod]
        public void DeleteUserById_EntityShouldBeDeletedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var entityId = Guid.NewGuid();
            mockRepository.Setup(p => p.GetEntityByID(entityId)).ReturnsAsync(serverEntity);
            mockRepository.Setup(p => p.DeleteEntity(entityId, true));

            var response = controller.Delete(entityId).Result;
            response.ErrorId.Should().Be(0);
            response.Should().NotBeNull();
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
