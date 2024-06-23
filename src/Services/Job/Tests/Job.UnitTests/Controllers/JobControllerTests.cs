using FluentAssertions;
using Jobs.Service.Common;
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
    public class JobControllerTests : BaseTestEntity<Job, JobController>
    {
        [Test]
        public async Task GetAllJobs_ResponseDataShouldContainAllCreatedEntities()
        {
            List<Job> entities = GetTestEntities();
            IEnumerable<JobDTO> dtoEntities = entities.Select(u => new JobDTO { Id = u.Id, Name = u.Name, Description = u.Description });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<JobDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = await controller.GetAll();
            response.Should().NotBeNull();

            var entitiesFromServer = response.Result as IEnumerable<JobDTO>;
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Should().HaveCount(dtoEntities.Count());
            entitiesFromServer.First().Id.Should().Be(dtoEntities.First().Id);
        }

        [Test]
        public async Task GetJobById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = await controller.Get(Guid.NewGuid());
            response.ErrorId.Should().Be(404);
        }

        [Test]
        public async Task GetJobById_ResponseDataShouldNotBeNull()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new JobDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            mockRepository.Setup(p => p.GetEntityByID(serverEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<JobDTO>(serverEntity)).Returns(clientEntity);

            var response = await controller.Get(serverEntity.Id);
            response.ErrorId.Should().Be(0);

            var entityFromServer = response.Result as JobDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [Test]
        public async Task CreateJob_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = await controller.Create(null);
            response.ErrorId.Should().Be(400);
        }

        [Test]
        public async Task CreateJob_EntityShouldBeCreatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new JobDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            mockRepository.Setup(p => p.InsertEntity(serverEntity, true)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<Job>(clientEntity)).Returns(serverEntity);
            mockMapper.Setup(p => p.Map<JobDTO>(serverEntity)).Returns(clientEntity);

            var response = await controller.Create(clientEntity);
            response.ErrorId.Should().Be(0);

            var entityFromServer = response.Result as JobDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [Test]
        public async Task UpdateJob_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = await controller.Update(null);
            response.ErrorId.Should().Be(400);
        }

        [Test]
        public async Task UpdateJob_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new JobDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            Job entity = null;
            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(entity);

            var response = await controller.Update(clientEntity);
            response.ErrorId.Should().Be(404);
        }

        [Test]
        public async Task UpdateJob_EntityShouldBeUpdatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new JobDTO() { Id = serverEntity.Id, Name = serverEntity.Name, Description = serverEntity.Description };

            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map(clientEntity, serverEntity)).Returns(serverEntity);
            mockRepository.Setup(p => p.UpdateEntity(serverEntity, true));

            var response = await controller.Update(clientEntity);
            response.ErrorId.Should().Be(0);
        }

        [Test]
        public async Task DeleteJobById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = await controller.Delete(Guid.NewGuid());
            response.ErrorId.Should().Be(404);
        }

        [Test]
        public async Task DeleteJobById_EntityShouldBeDeletedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var entityId = Guid.NewGuid();
            mockRepository.Setup(p => p.GetEntityByID(entityId)).ReturnsAsync(serverEntity);
            mockRepository.Setup(p => p.DeleteEntity(entityId, true));

            var response = await controller.Delete(entityId);
            response.ErrorId.Should().Be(0);
        }

        [Test]
        public async Task GetJobByCategoryId_ThereShouldBeNotBeMessageAndResultShouldbeEmpty_BecauseEntitiesNotFoundWithThisCategoryId()
        {
            List<Job> entities = GetTestEntities();
            IEnumerable<JobDTO> dtoEntities = entities.Select(u => new JobDTO { Id = u.Id, Name = u.Name, Description = u.Description });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<JobDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = await controller.GetJobsByCategoryId(Guid.NewGuid());
            response.Should().NotBeNull();
            response.ErrorId.Should().Be(0);

            var entitiesFromServer = response.Result as IEnumerable<JobDTO>;
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Should().BeEmpty();
        }

        [Test]
        public async Task GetJobByCategoryId_ResultShouldHaveTwoItems_BecauseWeCreatedTwoJobsWithThisCategoryId()
        {
            var categoryId = Guid.NewGuid();
            List<Job> entities = GetTestEntities();
            entities[0].CategoryId = categoryId;
            entities[1].CategoryId = categoryId;
            IEnumerable<JobDTO> dtoEntities = entities.Select(u => new JobDTO { Id = u.Id, Name = u.Name, Description = u.Description, CategoryId = u.CategoryId });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<JobDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = await controller.GetJobsByCategoryId(categoryId);
            var entitiesFromServer = response.Result as IEnumerable<JobDTO>;
            entitiesFromServer.Count().Should().BeLessThan(dtoEntities.Count());
            entitiesFromServer.Should().HaveCount(2);
        }

        List<Job> GetTestEntities()
        {
            return new List<Job>()
                        {
                            new Job
                            {
                                Name = "Back-end developer",
                                Description = "ASP.Net Core and Xamarin developer",
                                Cost = 25,
                                Type = JobType.Hourly,
                                Duration = JobDuration.FromOneToThreeMonths
                            },
                            new Job
                            {
                                Name = "Angular Developer Needed",
                                Description = "We need experienced Angular developer for short term project.",
                                Cost = 25,
                                Type = JobType.Hourly,
                                Duration = JobDuration.LessThanMonth
                            },
                            new Job
                            {
                                Name = "Salesperson",
                                Description = "Salesperson needed",
                                Cost = 2000,
                                Type = JobType.FixedPrice,
                                Duration = JobDuration.FromOneToThreeMonths
                            },
                            new Job
                            {
                                Name = "Design & Photography",
                                Description = "Design & Photography needed to build mockup of mobile app",
                                Cost = 30,
                                Type = JobType.Hourly,
                                Duration = JobDuration.MoreThanSixMonths
                            }
                        };
        }
    }
}
