using AutoMapper;
using EventBus.RabbitMQ;
using Jobs.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.DataProvider;
using PaymentService.Models;
using PaymentService.RabbitMQEvents.Events;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : BaseController<Payment, PaymentDTO>
    {
        private readonly IEventBusRabbitMQ _eventBus;

        public PaymentController(IJobsMongoContext context, IMapper mapper, IEventBusRabbitMQ eventBus) : base(context.PaymentRepository, mapper)
        {
            _eventBus = eventBus;
        }

        public override async Task<RequestModel> Create([FromBody] PaymentDTO entity)
        {
            if (entity == null)
                return await base.Create(entity);

            var userId = User?.GetUserId();
            entity.UserId = userId;
            entity.UserName = User?.GetUserName();

            var result = await base.Create(entity);

            if (result.ErrorId == 0 && userId is not null)
            {
                var newPayment = new UserPaymentEvent() { UserId = (Guid)userId, Amount = entity.Amount };
                _eventBus.Publish(newPayment);
            }

            return result;
        }

        [SwaggerResponse(501, "We will not support updating payment's information", typeof(RequestModel))]
        public override async Task<RequestModel> Update([FromBody] PaymentDTO entity)
        {
            return await RequestModel.ErrorRequestAsync("We will not support updating payment's information", 501);
        }

        [AllowAnonymous]
        [HttpGet("PaymentsByUserId/{userId}")]
        [SwaggerOperation(Summary = "To get payments by user Id")]
        [SwaggerResponse(200, "Return the found payments if it's finished successfully", typeof(RequestModel))]
        public virtual async Task<RequestModel> GetPaymentsByUserId(Guid userId)
        {
            var entities = (await _repository.GetEntities()).Where(e => e.UserId == userId).Select(e => _mapper.Map<PaymentDTO>(e));
            return await RequestModel.SuccessAsync(entities);
        }
    }
}