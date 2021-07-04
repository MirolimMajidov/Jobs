using AutoMapper;
using Jobs.Service.Common;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentService.DataProvider;
using PaymentService.Models;
using System;

namespace PaymentService.UnitTests
{
    public class BaseTestEntity<TEntity, TController>
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
            var context = new Mock<IJobsContext>();
            if (mockRepository.Object is IEntityRepository<Payment> repository)
                context.Setup(c => c.PaymentRepository).Returns(repository);

            controller = (TController)Activator.CreateInstance(typeof(TController), context.Object, mockMapper.Object);
        }

        public void Dispose()
        {
            mockRepository = null;
            mockMapper = null;
            controller = null;
        }
    }
}
