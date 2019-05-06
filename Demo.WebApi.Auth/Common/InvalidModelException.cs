using Demo.WebApi.Auth.Common.Extensions;
using System;
using System.Runtime.Serialization;
using System.Web.Http.ModelBinding;

namespace Demo.WebApi.Auth.Common
{
    [Serializable]
    public class InvalidModelException : Exception
    {
        public InvalidModelException()
        {
        }

        public InvalidModelException(ModelStateDictionary modelState) :
            base(string.Join(separator: string.Empty, values: modelState.GetModelStateErrors()))
        {
        }

        public InvalidModelException(string message) :
            base(message)
        {
        }

        public InvalidModelException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        protected InvalidModelException(SerializationInfo info, StreamingContext context) : 
            base(info, context)
        {
        }
    }
}