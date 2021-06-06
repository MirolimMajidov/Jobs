using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Service.SharedModel.Helpers;
using System.Net;

namespace Service.SharedModel.Exceptions
{
    public class JobExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<JobExceptionFilter> _logger;

        public JobExceptionFilter(ILogger<JobExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext exceptionContext)
        {
            var httpResponse = exceptionContext.HttpContext.Response;
            _logger.LogError(new EventId(exceptionContext.Exception.HResult), exceptionContext.Exception, exceptionContext.Exception.Message);

            object responseError = null;
            if (exceptionContext.Exception is JobException exception)
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