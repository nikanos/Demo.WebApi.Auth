using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Demo.WebApi.Auth.Controllers
{
    public class BaseApiController: ApiController
    {
        protected string AuthenticatedUsername
        {
            get
            {
                return Request.GetOwinContext().Authentication?.User.Identity.Name;
            }
        }
    }
}