using FluentAssertions;
using Moq;
using PaymentService.Controllers;
using PaymentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PaymentService.UnitTests
{
    public class PaymentControllerTests : BaseTestEntity<Payment, PaymentController>
    {
        [Fact]
        public async Task GetAllPayments_ResponseDataShouldContainAllCreatedEntities()
        {
            List<Payment> entities = GetTestEntities();
            IEnumerable<PaymentDTO> dtoEntities = entities.Select(u => new PaymentDTO { Id = u.Id, Amount = u.Amount, Sender = u.Sender });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<PaymentDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = await controller.GetAll();
            response.Should().NotBeNull();

            var entitiesFromServer = response.Result as IEnumerable<PaymentDTO>;
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Should().HaveCount(dtoEntities.Count());
            entitiesFromServer.First().Should().Equals(dtoEntities.First());
        }

        [Fact]
        public async Task GetPaymentById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = await controller.Get(Guid.NewGuid());
            Assert.Equal(404, response.ErrorId);
        }

        [Fact]
        public async Task GetPaymentById_ResponseDataShouldNotBeNull()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new PaymentDTO() { Id = serverEntity.Id, Amount = serverEntity.Amount, Sender = serverEntity.Sender };

            mockRepository.Setup(p => p.GetEntityByID(serverEntity.Id)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<PaymentDTO>(serverEntity)).Returns(clientEntity);

            var response = await controller.Get(serverEntity.Id);
            Assert.Equal(0, response.ErrorId);

            var entityFromServer = response.Result as PaymentDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [Fact]
        public async Task CreatePayment_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = await controller.Create(null);
            Assert.Equal(400, response.ErrorId);
        }

        [Fact]
        public async Task CreatePayment_EntityShouldBeCreatedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new PaymentDTO() { Id = serverEntity.Id, Amount = serverEntity.Amount, Sender = serverEntity.Sender };

            mockRepository.Setup(p => p.InsertEntity(serverEntity, true)).ReturnsAsync(serverEntity);
            mockMapper.Setup(p => p.Map<Payment>(clientEntity)).Returns(serverEntity);
            mockMapper.Setup(p => p.Map<PaymentDTO>(serverEntity)).Returns(clientEntity);

            var response = await controller.Create(clientEntity);
            Assert.Equal(0, response.ErrorId);

            var entityFromServer = response.Result as PaymentDTO;
            entityFromServer.Should().Be(clientEntity);
        }

        [Fact]
        public async Task UpdatePayment_ThereShouldBeErrorMessage_BecauseEntityCanNotBeNull()
        {
            var response = await controller.Update(null);
            Assert.Equal(501, response.ErrorId);
        }

        [Fact]
        public async Task UpdatePayment_ThereShouldBeErrorMessage_BecauseWeWillNotSupportUpdaingPaymentInfo()
        {
            var serverEntity = GetTestEntities().First();
            var clientEntity = new PaymentDTO() { Id = serverEntity.Id, Amount = serverEntity.Amount, Sender = serverEntity.Sender };

            Payment entity = null;
            mockRepository.Setup(p => p.GetEntityByID(clientEntity.Id)).ReturnsAsync(entity);

            var response = await controller.Update(clientEntity);
            Assert.Equal(501, response.ErrorId);
        }

        [Fact]
        public async Task DeletePaymentById_ThereShouldBeErrorMessage_BecauseEntityNotFoundWithThisId()
        {
            var response = await controller.Delete(Guid.NewGuid());
            response.ErrorId.Should().Be(404);
        }

        [Fact]
        public async Task DeletePaymentById_EntityShouldBeDeletedSuccessfully()
        {
            var serverEntity = GetTestEntities().First();
            var entityId = Guid.NewGuid();
            mockRepository.Setup(p => p.GetEntityByID(entityId)).ReturnsAsync(serverEntity);
            mockRepository.Setup(p => p.DeleteEntity(entityId, true));

            var response = await controller.Delete(entityId);
            response.ErrorId.Should().Be(0);
        }

        [Fact]
        public async Task GetPaymentByUserId_ThereShouldBeNotBeMessageAndResultShouldbeEmpty_BecauseEntitiesNotFoundWithThisCategoryId()
        {
            List<Payment> entities = GetTestEntities();
            IEnumerable<PaymentDTO> dtoEntities = entities.Select(u => new PaymentDTO { Id = u.Id, Amount = u.Amount, Sender = u.Sender });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<PaymentDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = await controller.GetPaymentsByUserId(Guid.NewGuid());
            response.Should().NotBeNull();
            response.ErrorId.Should().Be(0);

            var entitiesFromServer = response.Result as IEnumerable<PaymentDTO>;
            entitiesFromServer.Should().NotBeNull();
            entitiesFromServer.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPaymentByUserId_ResultShouldHaveTwoItems_BecauseWeCreatedTwoJobsWithThisCategoryId()
        {
            var userId = Guid.NewGuid();
            List<Payment> entities = GetTestEntities();
            entities[0].UserId = userId;
            entities[1].UserId = userId;
            IEnumerable<PaymentDTO> dtoEntities = entities.Select(u => new PaymentDTO { Id = u.Id, Amount = u.Amount, Sender = u.Sender });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<PaymentDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var response = await controller.GetPaymentsByUserId(userId);
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
