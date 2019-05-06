using Demo.WebApi.Auth.Common;
using System.Collections.Generic;

namespace Demo.WebApi.Auth.Services
{
    public class UserService : IUserService
    {
        private readonly List<string> adminUsers = new List<string>() { "admin", "administrator", "Administrator", "root", "sudo", "ninja" };//hardcoded list of administrators

        public IEnumerable<string> GetRolesByUsername(string userName)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(userName, nameof(userName));

            yield return "user";//all users have the role "user".
            if (adminUsers.Contains(userName))
                yield return "admin";//administrators have the role "admin"
        }

        public bool ValidateCredentials(string userName, string password)
        {
            Ensure.StringArgumentNotNullAndNotEmpty(userName, nameof(userName));
            Ensure.StringArgumentNotNullAndNotEmpty(password, nameof(password));

            //Dummy password check implementation. Normally we should call a user repository, but for this test just check that the password is the same as the username
            return userName == password;
        }
    }
}