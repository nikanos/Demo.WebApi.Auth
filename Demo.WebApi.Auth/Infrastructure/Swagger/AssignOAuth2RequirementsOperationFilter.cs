using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Http.Filters;

namespace Demo.WebApi.Auth.Infrastructure.Swagger
{
    public class AssignOAuth2RequirementsOperationFilter<TFilterAttribute> : IOperationFilter where TFilterAttribute : FilterAttribute
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            ICollection<FilterInfo> actionFilters = apiDescription.ActionDescriptor.GetFilterPipeline();
            var shouldAddOAuthRequirements = actionFilters.Any(x => x.Instance is TFilterAttribute);

            if (shouldAddOAuthRequirements)
            {
                if (operation.security == null)
                    operation.security = new List<IDictionary<string, IEnumerable<string>>>();
                var oAuthRequirements = new Dictionary<string, IEnumerable<string>>()
                    {
                        {"OAuth2", new string[] {"NBA Inspection Services APIs"}}
                    };
                operation.security.Add(oAuthRequirements);
            }
        }
    }
}