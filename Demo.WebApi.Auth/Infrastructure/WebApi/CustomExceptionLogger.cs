using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;

namespace Demo.WebApi.Auth.Infrastructure.WebApi
{
    class CustomExceptionLogger: ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Exception == null)
                throw new ArgumentException("property Excepttion cannot be null", nameof(context));

            if (context.ExceptionContext == null)
                throw new ArgumentException("property ExceptionContext cannot be null", nameof(context));

            if (context.Request == null)
                throw new ArgumentException("property Request cannot be null", nameof(context));

            if (context.RequestContext == null)
                throw new ArgumentException("property RequestContext cannot be null", nameof(context));

            string errorMessage = GenerateErrorMessage(context.Request, context.RequestContext, context.Exception);
            //log exception here
        }

        private string GenerateErrorMessage(HttpRequestMessage request, HttpRequestContext requestContext, Exception exception)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"CorrelationID: {request.GetCorrelationId()}");
            sb.AppendLine($"Uri: {request.RequestUri}");
            sb.AppendLine($"Method: {request.Method}");
            sb.AppendLine($"User: {requestContext.Principal.Identity.Name}");
            sb.AppendLine($"Exception: {exception.ToString()}");

            return sb.ToString();
        }
    }
}