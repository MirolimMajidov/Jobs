using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Jobs.Service.Common
{
    public class JobsExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<JobsExceptionFilter> _logger;

        public JobsExceptionFilter(ILogger<JobsExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext exceptionContext)
        {
            var httpResponse = exceptionContext.HttpContext.Response;
            _logger.LogError(new EventId(exceptionContext.Exception.HResult), exceptionContext.Exception, exceptionContext.Exception.Message);

            object responseError = null;
            if (exceptionContext.Exception is JobsException exception)
            {
                responseError = new { ExceptionMessage = exception.Message };
                httpResponse.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
#if DEBUG
                responseError = new { ExceptionMessage = exceptionContext.Exception.Message, exceptionContext.Exception };
#else
                responseError = new { ExceptionMessage = "An error on executing request." };
#endif
                httpResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            httpResponse.ContentType = "application/json";
            exceptionContext.Result = new BadRequestObjectResult(RequestModel.GenaretJson(responseError));
            exceptionContext.ExceptionHandled = true;
        }
    }
}