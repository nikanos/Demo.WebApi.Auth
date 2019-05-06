using Demo.WebApi.Auth.Common;
using Demo.WebApi.Auth.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Demo.WebApi.Auth.Infrastructure.WebApi
{
    public class CustomExceptionHandler: ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Exception == null)
                throw new ArgumentException("property Exception cannot be null", nameof(context));

            if (context.ExceptionContext == null)
                throw new ArgumentException("property ExceptionContext cannot be null", nameof(context));

            if (context.Request == null)
                throw new ArgumentException("property Request cannot be null", nameof(context));

            if (context.RequestContext == null)
                throw new ArgumentException("property RequestContext cannot be null", nameof(context));

            if (context.ExceptionContext.CatchBlock.Equals(ExceptionCatchBlocks.IExceptionFilter))
                return;

            context.Result = CreateErrorResult(context.Request, context.Exception);
        }

        private IHttpActionResult CreateErrorResult(HttpRequestMessage request, Exception exception)
        {
            Dictionary<string, object> responseContent = new Dictionary<string, object>();
            responseContent.Add(ErrorResponseFields.ERROR_TYPE, ErrorType.Unhandled.ToString());
            responseContent.Add(ErrorResponseFields.MESSAGE, Constants.GENERIC_ERROR_MESSAGE);
            responseContent.Add(ErrorResponseFields.CORRELATION_ID, request.GetCorrelationId().ToString());

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError; // set the http status code as InternalServerError (500) by default

            if (exception is InvalidModelException)
            {
                responseContent[ErrorResponseFields.ERROR_TYPE] = ErrorType.InvalidModel.ToString();
                responseContent[ErrorResponseFields.MESSAGE] = exception.Message;
                statusCode = HttpStatusCode.BadRequest;
            }

            // Add ExceptionDetails if configured to do so
            if (request.ShouldIncludeErrorDetail())
                responseContent.Add(ErrorResponseFields.EXCEPTION_DETAILS, exception.ToString());

            return new ResponseMessageResult(request.CreateResponse(statusCode, responseContent));
        }
    }
}