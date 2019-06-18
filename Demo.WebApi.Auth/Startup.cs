using Demo.WebApi.Auth.Infrastructure.OAuth;
using Demo.WebApi.Auth.Infrastructure.WebApi;
using Demo.WebApi.Auth.Services;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Swashbuckle.Application;
using System;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

[assembly: OwinStartup(typeof(Demo.WebApi.Auth.Startup))]
namespace Demo.WebApi.Auth
{
    public class Startup
    {
        private const string PRODUCTTION_ENVIRONMENT = "RELEASE";
        private const int PRODUCTTION_MAX_TOKEN_VALIDITY_DAYS = 1;
        private const string APPLICATION_PATH_IIS_HOSTED = "%IIS_HOSTED%";

        private StartupConfig config;

        public void Configuration(IAppBuilder app)
        {
            config = new StartupConfig();

            // Please note that we FIRST have to configure OAuth and THEN WebApi. So the below call order is important
            ConfigureOAuth(app);
            ConfigureWebApi(app);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            // Make sure http is not allowed for production environment
            if (config.OAuthAllowHttp && config.RunningEnvironment == PRODUCTTION_ENVIRONMENT)
                throw new StartupConfigException("OAuthAllowHttp should not be enabled!!!");

            // Make sure token validity is not greater than a hard constant for production environment (to prevent accidentally issuing tokens with long validity)
            if (config.OAuthTokenValidityPeriod.Days > PRODUCTTION_MAX_TOKEN_VALIDITY_DAYS && config.RunningEnvironment == PRODUCTTION_ENVIRONMENT)
                throw new StartupConfigException($"OAuthTokenValidityPeriod should not be greater than {PRODUCTTION_MAX_TOKEN_VALIDITY_DAYS}!!!");

            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = config.OAuthAllowHttp,
                TokenEndpointPath = new PathString(config.OAuthTokenEndpoint),
                AccessTokenExpireTimeSpan = config.OAuthTokenValidityPeriod,
                Provider = new CustomOAuthProvider(new UserService())// Use our custom OAuth provider
            };
            app.UseOAuthBearerTokens(oAuthServerOptions);
        }

        private void ConfigureWebApi(IAppBuilder app)
        {
            // Make sure IncludeExceptionDetails is not enabled for production environment
            if (config.WebApiIncludeExceptionDetails && config.RunningEnvironment == PRODUCTTION_ENVIRONMENT)
                throw new StartupConfigException("WebApiIncludeExceptionDetails should not be enabled!!!");

            HttpConfiguration httpConfig = new HttpConfiguration();

            httpConfig.Formatters.Clear();
            httpConfig.Formatters.Add(new JsonMediaTypeFormatter()); // Keep only JSON formatter

            // Assign IncludeErrorDetailPolicy based on our option in config
            httpConfig.IncludeErrorDetailPolicy = config.WebApiIncludeExceptionDetails ? IncludeErrorDetailPolicy.Always : IncludeErrorDetailPolicy.Never;

            // Add our custom exception logger. We can have many loggers.
            httpConfig.Services.Add(typeof(IExceptionLogger), new CustomExceptionLogger());

            // Replace the default exception handler. We use Replace as there can only be one exception handler.
            httpConfig.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());

            httpConfig.MapHttpAttributeRoutes(); // Use attribute-based routing

            // Default webapi conventional-based routing (commented out since we want only attribute-based routing!!!)
            // httpConfig.Routes.MapHttpRoute(name:="DefaultApi", routeTemplate:="api/{controller}/{id}", defaults:=New With {.id = RouteParameter.Optional})

            app.UseWebApi(httpConfig);

            if (config.RunningEnvironment != PRODUCTTION_ENVIRONMENT)
            {
                // We enable swagger help page for non-production environments
                ConfigureSwagger(httpConfig);
            }
        }

        private void ConfigureSwagger(HttpConfiguration httpConfig)
        {
            string applicationPath = config.ApplicationPath;

            if (applicationPath == APPLICATION_PATH_IIS_HOSTED)
                // If we are hosted under IIS we can safely use VirtualPathUtility. In all other cases we use the config value
                applicationPath = System.Web.VirtualPathUtility.ToAbsolute("~");

            httpConfig.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Demo WebApi Auth Services");
                c.OAuth2("OAuth2").Description("Resource owner password credentials").Flow("password").TokenUrl(UrlCombineLib.UrlCombine.Combine(applicationPath, config.OAuthTokenEndpoint));
                c.OperationFilter(() => new Infrastructure.Swagger.AssignOAuth2RequirementsOperationFilter<AuthorizeAttribute>());
                c.RootUrl(req =>
                {
                    string rootUrl = UrlCombineLib.UrlCombine.Combine(req.RequestUri.GetLeftPart(UriPartial.Authority), applicationPath);
                    // Swashbuckle does NOT like trailing slashes
                    return rootUrl.TrimEnd('/');
                });                
            }).EnableSwaggerUi(x =>
            {
                x.EnableDiscoveryUrlSelector();
                x.DisableValidator();
            });
        }
    }
}
