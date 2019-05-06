using System;
using System.Runtime.Serialization;

namespace Demo.WebApi.Auth
{
    [Serializable]
    internal class StartupConfigException : Exception
    {
        public StartupConfigException()
        {
        }

        public StartupConfigException(string message) : base(message)
        {
        }

        public StartupConfigException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StartupConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}