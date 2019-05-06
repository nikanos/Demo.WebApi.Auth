using Demo.WebApi.Auth.Common;
using Demo.WebApi.Auth.Services;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.WebApi.Auth.Infrastructure.OAuth
{
    class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        #region OAuthErrors

        /// <summary>
        /// OAuth Errors
        /// See RFC 6749
        /// https://tools.ietf.org/html/rfc6749#section-5.2
        /// </summary>
        private class OAuthErrors
        {
            /// <summary>
            /// The provided authorization grant (e.g., authorization code, resource owner credentials) Or
            /// refresh token Is invalid, expired, revoked, does Not match the redirection URI used In the authorization request, Or was issued To another client.
            /// </summary>
            public const string INVALID_GRANT = "invalid_grant";

            /// <summary>
            /// The request is missing a required parameter, includes an unsupported parameter value (other than grant type),repeats a parameter,
            /// includes multiple credentials, utilizes more than one mechanism For authenticating the client, Or Is otherwise malformed.
            /// </summary>
            public const string INVALID_REQUEST = "invalid_request";
        }

        #endregion

        private readonly IUserService userService;
        public CustomOAuthProvider(IUserService userService)
        {
            Ensure.ArgumentNotNull(userService, nameof(userService));

            this.userService = userService;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userName = context.UserName;
            var password = context.Password;

            if (string.IsNullOrEmpty(userName))
            {
                context.SetError(OAuthErrors.INVALID_REQUEST, "User name is missing");
                return base.GrantResourceOwnerCredentials(context);
            }

            if (string.IsNullOrEmpty(password))
            {
                context.SetError(OAuthErrors.INVALID_REQUEST, "Password is missing");
                return base.GrantResourceOwnerCredentials(context);
            }

            if (userService.ValidateCredentials(userName, password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName)
                };

                IEnumerable<string> userRoles = userService.GetRolesByUsername(userName);
                foreach (string userRole in userRoles)
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims, context.Options.AuthenticationType);
                context.Validated(oAuthIdentity);
            }
            else
                context.SetError(OAuthErrors.INVALID_GRANT, "The user name or password is incorrect");

            return base.GrantResourceOwnerCredentials(context);
        }
    }
}