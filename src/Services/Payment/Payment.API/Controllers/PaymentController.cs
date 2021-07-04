using AutoMapper;
using Jobs.Service.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.DataProvider;
using PaymentService.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : BaseController<Payment, PaymentDTO>
    {
        public PaymentController(IJobsContext context, IMapper mapper) : base(context.PaymentRepository, mapper) { }

        public override async Task<RequestModel> Create([FromBody] PaymentDTO entity)
        {
            if (entity == null)
                return await base.Create(entity);

            entity.UserId = User?.GetUserId();
            entity.UserName = User?.GetUserName();

            return await base.Create(entity);
        }

        [SwaggerResponse(501, "We will not support updating paymnet's information", typeof(RequestModel))]
        public override async Task<RequestModel> Update([FromBody] PaymentDTO entity)
        {
            return await RequestModel.ErrorRequestAsync("We will not support updating paymnet's information", 501);
        }

        [AllowAnonymous]
        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "To get payments by user Id")]
        [SwaggerResponse(200, "Return the found payments if it's finished successfully", typeof(RequestModel))]
        public virtual async Task<RequestModel> GetPaymentsByUserId(Guid userId)
        {
            var entities = (await _repository.GetEntities()).Where(e => e.UserId == userId).Select(e => _mapper.Map<PaymentDTO>(e));
            return await RequestModel.SuccessAsync(entities);
        }
    }
}