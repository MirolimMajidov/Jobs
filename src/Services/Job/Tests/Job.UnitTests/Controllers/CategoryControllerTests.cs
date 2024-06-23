using FluentAssertions;
using JobService.Controllers;
using JobService.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobService.UnitTests
{
    [TestFixture]
    public class CategoryControllerTests : BaseTestEntity<Category, CategoryController>
    {
        [Test]
        public async Task GetAllCategories_ResponseDataShouldContainAllCreatedEntities()
        {
            List<Category> entities = GetTestEntities();
            IEnumerable<CategoryDTO> dtoEntities = entities.Select(u => new CategoryDTO { Id = u.Id, Name = u.Name, Description = u.Description });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<CategoryDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = await controller.GetAll();
            response.Should().NotBeNull();

            var entitiesFromServer = response.Result as IEnumerable<CategoryDTO>;
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Should().HaveCount(dtoEntities.Count());
            entitiesFromServer.First().Id.Should().Be(dtoEntities.First().Id);
        }

        [Test]
        public async Task GetCategoryById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = await controller.Get(Guid.NewGuid());
            response.ErrorId.Should().Be(404);
        }

        [Test]
        public async Task GetCategoryById_ResponseDataShouldNotBeNull()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new CategoryDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            mockRepository.Setup(p => p.GetEntityByID(serverEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<CategoryDTO>(serverEntity)).Returns(clientEntity);

            var response = await controller.Get(serverEntity.Id);
            response.ErrorId.Should().Be(0);

            var entityFromServer = response.Result as CategoryDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [Test]
        public async Task CreateCategory_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = await controller.Create(null);
            response.ErrorId.Should().Be(400);
        }

        [Test]
        public async Task CreateCategory_EntityShouldBeCreatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new CategoryDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            mockRepository.Setup(p => p.InsertEntity(serverEntity, true)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<Category>(clientEntity)).Returns(serverEntity);
            mockMapper.Setup(p => p.Map<CategoryDTO>(serverEntity)).Returns(clientEntity);

            var response = await controller.Create(clientEntity);
            response.ErrorId.Should().Be(0);

            var entityFromServer = response.Result as CategoryDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [Test]
        public async Task UpdateCategory_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = await controller.Update(null);
            response.ErrorId.Should().Be(400);
        }

        [Test]
        public async Task UpdateCategory_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new CategoryDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            Category entity = null;
            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(entity);

            var response = await controller.Update(clientEntity);
            response.ErrorId.Should().Be(404);
        }

        [Test]
        public async Task UpdateCategory_EntityShouldBeUpdatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new CategoryDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map(clientEntity, serverEntity)).Returns(serverEntity);
            mockRepository.Setup(p => p.UpdateEntity(serverEntity, true));

            var response = await controller.Update(clientEntity);
            response.ErrorId.Should().Be(0);
        }

        [Test]
        public async Task DeleteCategoryById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = await controller.Delete(Guid.NewGuid());
            response.ErrorId.Should().Be(404);
        }

        [Test]
        public async Task DeleteCategoryById_EntityShouldBeDeletedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var entityId = Guid.NewGuid();
            mockRepository.Setup(p => p.GetEntityByID(entityId)).ReturnsAsync(serverEntity);
            mockRepository.Setup(p => p.DeleteEntity(entityId, true));

            var response = await controller.Delete(entityId);
            response.ErrorId.Should().Be(0);
        }

        List<Category> GetTestEntities()
        {
            return new List<Category>()
                        {
                            new Category
                            {
                                Name = "Web, Mobile & Software Dev",
                                Description = "Web Development, Mobile Development, Desktop Software Developmen, QA & Testing",
                            },
                            new Category
                            {
                                Name = "Sales & Marketing",
                                Description = "Sales & Marketing Strategy",
                            },
                            new Category
                            {
                                Name = "Design & Writing",
                                Description = "Design, Writing, Photography & Translator",
                            },
                            new Category
                            {
                                Name = "Engineering & Architecture",
                                Description = "Engineering & Architecture",
                            }
                        };
        }
    }
}
