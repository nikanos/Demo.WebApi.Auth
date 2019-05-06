using Demo.WebApi.Auth.Common;
using System;
using System.Configuration;
using System.Globalization;

namespace Demo.WebApi.Auth
{
    class StartupConfig
    {
        public StartupConfig()
        {
            OAuthTokenEndpoint = ReadConfigStringValue("oauth:TokenEndpoint");
            OAuthTokenValidityPeriod = ReadConfigTimeSpanValue("oauth:TokenValidityPeriod");
            OAuthAllowHttp = ReadConfigBooleanValue("oauth:AllowHttp");
            WebApiIncludeExceptionDetails = ReadConfigBooleanValue("webapi:IncludeExceptionDetails");
            RunningEnvironment = ReadConfigStringValue("env:RunningEnvironment");
            ApplicationPath = ReadConfigStringValue("env:ApplicationPath");
        }

        public string OAuthTokenEndpoint { get; private set; }
        public TimeSpan OAuthTokenValidityPeriod { get; private set; }
        public bool OAuthAllowHttp { get; private set; }
        public bool WebApiIncludeExceptionDetails { get; private set; }
        public string RunningEnvironment { get; private set; }
        public string ApplicationPath { get; private set; }

        private string ReadConfigStringValue(string key)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(key, nameof(key));

            string value = ConfigurationManager.AppSettings[key];

            if (value == null)
                throw new StartupConfigException($"No config value found for key {key}");

            if (value.Length == 0)
                throw new StartupConfigException($"config value found for key {key} was empty");

            return value;
        }

        private TimeSpan ReadConfigTimeSpanValue(string key)
        {
            string value = ReadConfigStringValue(key);
            var parseSuccess = TimeSpan.TryParseExact(value, new[] { @"hh\:mm\:ss", @"d\.hh\:mm\:ss" }, CultureInfo.InvariantCulture, out TimeSpan result);

            if (!parseSuccess)
                throw new StartupConfigException($"Invalid Timespan config value {value} for key {key}");

            return result;
        }

        private bool ReadConfigBooleanValue(string key)
        {
            string value = ReadConfigStringValue(key);
            var parseSuccess = bool.TryParse(value, out bool result);

            if (!parseSuccess)
                throw new StartupConfigException($"Invalid Boolean config value {value} for key {key}");

            return result;
        }
    }
}