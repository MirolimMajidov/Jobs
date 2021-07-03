using AutoMapper;
using Jobs.Common.Models;
using Jobs.Service.Common.Repository;
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
        protected Mock<JobsContext> context;
        protected TController controller;

        public BaseTestEntity()
        {
            mockRepository = new Mock<IEntityRepository<TEntity>>();
            mockMapper = new Mock<IMapper>();
            context = new Mock<JobsContext>(null);
            if (mockRepository.Object is IEntityRepository<Payment> repository)
                context.Object.PaymentRepository = repository;

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
