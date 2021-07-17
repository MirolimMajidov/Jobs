using AutoMapper;
using EventBus.RabbitMQ;
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
        protected Mock<IEventBusRabbitMQ> mockEventBus;
        protected TController controller;

        public BaseTestEntity()
        {
            mockRepository = new Mock<IEntityRepository<TEntity>>();
            mockMapper = new Mock<IMapper>();
            mockEventBus = new Mock<IEventBusRabbitMQ>();
            var context = new Mock<IJobsMongoContext>();
            if (mockRepository.Object is IEntityRepository<Payment> repository)
                context.Setup(c => c.PaymentRepository).Returns(repository);

            controller = (TController)Activator.CreateInstance(typeof(TController), context.Object, mockMapper.Object, mockEventBus.Object);
        }

        public void Dispose()
        {
            mockRepository = null;
            mockMapper = null;
            controller = null;
        }
    }
}
