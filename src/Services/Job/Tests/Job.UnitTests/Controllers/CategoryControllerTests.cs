using FluentAssertions;
using JobService.Controllers;
using JobService.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobService.UnitTests
{
    [TestFixture]
    public class CategoryControllerTests : BaseTestEntity<Category, CategoryController>
    {
        [Test]
        public void GetAllCategories_ResponseDataShouldContainAllCreatedEntities()
        {
            List<Category> entities = GetTestEntities();
            IEnumerable<CategoryDTO> dtoEntities = entities.Select(u => new CategoryDTO { Id = u.Id, Name = u.Name, Description = u.Description });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<CategoryDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = controller.GetAll().Result;
            response.Should().NotBeNull();

            var entitiesFromServer = response.Result as IEnumerable<CategoryDTO>;
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Should().HaveCount(dtoEntities.Count());
            entitiesFromServer.First().Should().Equals(dtoEntities.First());
        }

        [Test]
        public void GetCategoryById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = controller.Get(Guid.NewGuid()).Result;
            response.ErrorId.Equals(404);
        }

        [Test]
        public void GetCategoryById_ResponseDataShouldNotBeNull()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new CategoryDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            mockRepository.Setup(p => p.GetEntityByID(serverEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<CategoryDTO>(serverEntity)).Returns(clientEntity);

            var response = controller.Get(serverEntity.Id).Result;
            response.ErrorId.Equals(0);
            response.Should().NotBeNull();

            var entityFromServer = response.Result as CategoryDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [Test]
        public void CreateCategory_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = controller.Create(null).Result;
            response.ErrorId.Equals(400);
        }

        [Test]
        public void CreateCategory_EntityShouldBeCreatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new CategoryDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            mockRepository.Setup(p => p.InsertEntity(serverEntity, true)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<Category>(clientEntity)).Returns(serverEntity);
            mockMapper.Setup(p => p.Map<CategoryDTO>(serverEntity)).Returns(clientEntity);

            var response = controller.Create(clientEntity).Result;
            response.ErrorId.Equals(0);
            response.Should().NotBeNull();

            var entityFromServer = response.Result as CategoryDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [Test]
        public void UpdateCategory_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = controller.Update(null).Result;
            response.ErrorId.Equals(400);
        }

        [Test]
        public void UpdateCategory_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new CategoryDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            Category entity = null;
            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(entity);

            var response = controller.Update(clientEntity).Result;
            response.ErrorId.Equals(404);
        }

        [Test]
        public void UpdateCategory_EntityShouldBeUpdatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new CategoryDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map(clientEntity, serverEntity)).Returns(serverEntity);
            mockRepository.Setup(p => p.UpdateEntity(serverEntity, true));

            var response = controller.Update(clientEntity).Result;
            response.ErrorId.Equals(0);
            response.Should().NotBeNull();
        }

        [Test]
        public void DeleteCategoryById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = controller.Delete(Guid.NewGuid()).Result;
            response.ErrorId.Equals(404);
        }

        [Test]
        public void DeleteCategoryById_EntityShouldBeDeletedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var entityId = Guid.NewGuid();
            mockRepository.Setup(p => p.GetEntityByID(entityId)).ReturnsAsync(serverEntity);
            mockRepository.Setup(p => p.DeleteEntity(entityId, true));

            var response = controller.Delete(entityId).Result;
            response.ErrorId.Equals(0);
            response.Should().NotBeNull();
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
