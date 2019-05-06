using Demo.WebApi.Auth.Common;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Demo.WebApi.Auth.Infrastructure.WebApi
{
    public class ModelStateValidationFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
                throw new InvalidModelException(actionContext.ModelState);
        }
    }
}