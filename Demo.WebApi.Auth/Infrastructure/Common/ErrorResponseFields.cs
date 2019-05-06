using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.WebApi.Auth.Infrastructure.Common
{
    class ErrorResponseFields
    {
        public const string ERROR_TYPE = "ErrorType"; // Should be included in all errors
        public const string MESSAGE = "Message"; // Should be included in all errors        
        public const string CORRELATION_ID = "CorrelationID"; // Should be included in WEB API errors
        public const string EXCEPTION_DETAILS = "ExceptionDetails"; // 'Should be included in WEB API errors only if IncludeExceptionDetails is true
    }
}