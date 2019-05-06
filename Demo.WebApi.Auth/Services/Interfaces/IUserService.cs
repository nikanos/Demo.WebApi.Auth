using System.Collections.Generic;

namespace Demo.WebApi.Auth.Services
{
    public interface IUserService
    {
        bool ValidateCredentials(string userName, string password);
        IEnumerable<string> GetRolesByUsername(string userName);
    }
}
