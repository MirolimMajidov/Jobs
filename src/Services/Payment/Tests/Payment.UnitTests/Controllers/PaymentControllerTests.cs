using FluentAssertions;
using Moq;
using PaymentService.Controllers;
using PaymentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PaymentService.UnitTests
{
    public class PaymentControllerTests : BaseTestEntity<Payment, PaymentController>
    {
        [Fact]
        public void GetAllPayments_ResponseDataShouldContainAllCreatedEntities()
        {
            List<Payment> entities = GetTestEntities();
            IEnumerable<PaymentDTO> dtoEntities = entities.Select(u => new PaymentDTO { Id = u.Id, Amount = u.Amount, Sender = u.Sender });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<PaymentDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = controller.GetAll().Result;
            response.Should().NotBeNull();

            var entitiesFromServer = response.Result as IEnumerable<PaymentDTO>;
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Should().HaveCount(dtoEntities.Count());
            entitiesFromServer.First().Should().Equals(dtoEntities.First());
        }

        [Fact]
        public void GetPaymentById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = controller.Get(Guid.NewGuid()).Result;
            response.ErrorId.Equals(404);
        }

        [Fact]
        public void GetPaymentById_ResponseDataShouldNotBeNull()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new PaymentDTO() { Id = serverEntity.Id, Amount = serverEntity.Amount, Sender = serverEntity.Sender };

            mockRepository.Setup(p => p.GetEntityByID(serverEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<PaymentDTO>(serverEntity)).Returns(clientEntity);

            var response = controller.Get(serverEntity.Id).Result;
            response.ErrorId.Equals(0);
            response.Should().NotBeNull();

            var entityFromServer = response.Result as PaymentDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [Fact]
        public void CreatePayment_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = controller.Create(null).Result;
            response.ErrorId.Equals(400);
        }

        [Fact]
        public void CreatePayment_EntityShouldBeCreatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new PaymentDTO() { Id = serverEntity.Id, Amount = serverEntity.Amount, Sender = serverEntity.Sender };

            mockRepository.Setup(p => p.InsertEntity(serverEntity, true)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<Payment>(clientEntity)).Returns(serverEntity);
            mockMapper.Setup(p => p.Map<PaymentDTO>(serverEntity)).Returns(clientEntity);

            var response = controller.Create(clientEntity).Result;
            response.ErrorId.Equals(0);
            response.Should().NotBeNull();

            var entityFromServer = response.Result as PaymentDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [Fact]
        public void UpdatePayment_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = controller.Update(null).Result;
            response.ErrorId.Equals(400);
        }

        [Fact]
        public void UpdatePayment_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new PaymentDTO() { Id = serverEntity.Id, Amount = serverEntity.Amount, Sender = serverEntity.Sender };

            Payment entity = null;
            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(entity);

            var response = controller.Update(clientEntity).Result;
            response.ErrorId.Equals(404);
        }

        [Fact]
        public void UpdatePayment_EntityShouldBeUpdatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new PaymentDTO() { Id = serverEntity.Id, Amount = serverEntity.Amount, Sender = serverEntity.Sender };

            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map(clientEntity, serverEntity)).Returns(serverEntity);
            mockRepository.Setup(p => p.UpdateEntity(serverEntity, true));

            var response = controller.Update(clientEntity).Result;
            response.ErrorId.Equals(0);
            response.Should().NotBeNull();
        }

        [Fact]
        public void DeletePaymentById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = controller.Delete(Guid.NewGuid()).Result;
            response.ErrorId.Equals(404);
        }

        [Fact]
        public void DeletePaymentById_EntityShouldBeDeletedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var entityId = Guid.NewGuid();
            mockRepository.Setup(p => p.GetEntityByID(entityId)).ReturnsAsync(serverEntity);
            mockRepository.Setup(p => p.DeleteEntity(entityId, true));

            var response = controller.Delete(entityId).Result;
            response.ErrorId.Equals(0);
            response.Should().NotBeNull();
        }

        [Fact]
        public void GetJobByCategoryId_ThereShouldBeNotBeMessageAndResultShouldbeEmpty_BecauseEntitiesNotFoundWithThisCategoryId()
        {
            List<Payment> entities = GetTestEntities();
            IEnumerable<PaymentDTO> dtoEntities = entities.Select(u => new PaymentDTO { Id = u.Id, Amount = u.Amount, Sender = u.Sender });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<PaymentDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = controller.GetPaymentsByUserId(Guid.NewGuid()).Result;
            response.Should().NotBeNull();
            response.ErrorId.Should().Equals(0);

            var entitiesFromServer = response.Result as IEnumerable<PaymentDTO>;
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Should().BeEmpty();
        }

        [Fact]
        public void GetJobByCategoryId_ResultShouldHaveTwoItems_BecauseWeCreatedTwoJobsWithThisCategoryId()
        {
            var userId = Guid.NewGuid();
            List<Payment> entities = GetTestEntities();
            entities[0].UserId = userId;
            entities[1].UserId = userId;
            IEnumerable<PaymentDTO> dtoEntities = entities.Select(u => new PaymentDTO { Id = u.Id, Amount = u.Amount, Sender = u.Sender });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<PaymentDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = controller.GetPaymentsByUserId(userId).Result;
            var entitiesFromServer = response.Result as IEnumerable<PaymentDTO>;
            entitiesFromServer.Count().Should().BeLessThan(dtoEntities.Count());
            entitiesFromServer.Should().HaveCount(2);
        }

        List<Payment> GetTestEntities()
        {
            var payments = new List<Payment>();

            for (int i = 1; i < 5; i++)
            {
                var payment = new Payment
                {
                    OrderId = i.ToString(),
                    Amount = 100 * i,
                    Sender = "Payoneer"
                };
                payments.Add(payment);
            }

            return payments;
        }
    }
}
