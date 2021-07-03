using AutoMapper;
using Jobs.Service.Common.Controllers;
using Jobs.Service.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using PaymentService.DataProvider;
using PaymentService.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : BaseController<Payment, PaymentDTO>
    {
        public PaymentController(IJobsContext context, IMapper mapper) : base(context.PaymentRepository, mapper) { }

        public override async Task<RequestModel> Post([FromBody] PaymentDTO entity)
        {
            entity.UserId = User.GetUserId();
            entity.UserName = User.GetUserName();

            return await base.Post(entity);
        }

        [SwaggerResponse(501, "We will not support updating paymnet's information", typeof(RequestModel))]
        public override async Task<RequestModel> Put([FromBody] PaymentDTO entity)
        {
            return await RequestModel.ErrorRequestAsync("We will not support updating paymnet's information", 501);
        }
    }
}