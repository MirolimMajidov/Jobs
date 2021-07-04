using AutoMapper;
using Jobs.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;

namespace JobService.UnitTests
{
    [TestFixture]
    public abstract class BaseTestEntity<TEntity, TController>
        where TEntity : IEntity
        where TController : ControllerBase
    {
        protected Mock<IEntityRepository<TEntity>> mockRepository;
        protected Mock<IMapper> mockMapper;
        protected TController controller;

        public BaseTestEntity()
        {
            mockRepository = new Mock<IEntityRepository<TEntity>>();
            mockMapper = new Mock<IMapper>();
            controller = (TController)Activator.CreateInstance(typeof(TController), mockRepository.Object, mockMapper.Object);
        }

        [OneTimeSetUp]
        public void AssemblyInitialize()
        {
            //Executes once before the test run. (Optional)
        }

        [OneTimeTearDown]
        public void AssemblyCleanup()
        {
            //Executes once after the test run. (Optional)

            mockRepository = null;
            mockMapper = null;
            controller = null;
        }

        [SetUp]
        public void SetUp()
        {
            //Runs before each test. (Optional)
        }

        [TearDown]
        public void TearDown()
        {
            //Runs after each test. (Optional)
        }
    }
}
