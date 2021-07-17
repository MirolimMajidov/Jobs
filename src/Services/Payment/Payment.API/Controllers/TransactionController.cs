using AutoMapper;
using EventBus.RabbitMQ;
using Jobs.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;
using PaymentService.RabbitMQEvents.Events;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : BaseController<Transaction, TransactionDTO>
    {
        private readonly IEventBusRabbitMQ _eventBus;

        public TransactionController(IEntityRepository<Transaction> repository, IMapper mapper, IEventBusRabbitMQ eventBus) : base(repository, mapper)
        {
            _eventBus = eventBus;
        }

        public override async Task<RequestModel> Create([FromBody] TransactionDTO entity)
        {
            if (entity == null)
                return await base.Create(entity);

            entity.UserId = User?.GetUserId();
            entity.UserName = User?.GetUserName();

            var result = await base.Create(entity);

            if (result.ErrorId == 0)
            {
                var newPayment = new UserTransactionEvent() { UserId = entity.Id, Amount = entity.Amount };
                _eventBus.Publish(newPayment);
            }

            return result;
        }

        [SwaggerResponse(501, "We will not support updating transaction's information", typeof(RequestModel))]
        public override async Task<RequestModel> Update([FromBody] TransactionDTO entity)
        {
            return await RequestModel.ErrorRequestAsync("We will not support updating transaction's information", 501);
        }

        [AllowAnonymous]
        [HttpGet("TransactionsByUserId/{userId}")]
        [SwaggerOperation(Summary = "To get transactions by user Id")]
        [SwaggerResponse(200, "Return the found transactions if it's finished successfully", typeof(RequestModel))]
        public virtual async Task<RequestModel> GetTransactionsByUserId(Guid userId)
        {
            var entities = (await _repository.GetEntities()).Where(e => e.UserId == userId).Select(e => _mapper.Map<TransactionDTO>(e));
            return await RequestModel.SuccessAsync(entities);
        }
    }
}