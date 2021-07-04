using AutoMapper;
using EventBus.RabbitMQ;
using IdentityService.Controllers;
using Jobs.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace IdentityService.UnitTests
{
    public abstract class BaseTestEntity<TEntity, TController>
        where TEntity : IEntity
        where TController : ControllerBase
    {
        protected Mock<IEntityRepository<TEntity>> mockRepository;
        protected Mock<IMapper> mockMapper;
        protected Mock<IEventBusRabbitMQ> mockEventBus;
        protected TController controller;

        public BaseTestEntity()
        {
            mockRepository = new Mock<IEntityRepository<TEntity>>();
            mockMapper = new Mock<IMapper>();
            mockEventBus = new Mock<IEventBusRabbitMQ>();

            if (typeof(TController) == typeof(UserController))
            {
                controller = (TController)Activator.CreateInstance(typeof(TController), mockRepository.Object, mockMapper.Object, mockEventBus.Object);
            }
            else
                controller = (TController)Activator.CreateInstance(typeof(TController), mockRepository.Object, mockMapper.Object);
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            //Executes once before the test run. (Optional)
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            //Executes once after the test run. (Optional)
        }

        [TestInitialize]
        public void SetUp()
        {
            //Runs before each test. (Optional)
        }

        [TestCleanup]
        public void TearDown()
        {
            //Runs after each test. (Optional)
        }
    }
}
