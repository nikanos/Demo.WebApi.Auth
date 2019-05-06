using System.Collections.Generic;
using System.Web.Http.ModelBinding;

namespace Demo.WebApi.Auth.Common.Extensions
{
    static class ModelExtensions
    {
        public static IList<string> GetModelStateErrors(this ModelStateDictionary modelState)
        {
            Ensure.ArgumentNotNull(modelState, nameof(modelState));

            List<string> errors = new List<string>();
            foreach (var state in modelState.Values)
            {
                foreach (var modelError in state.Errors)
                {
                    if (!string.IsNullOrEmpty(modelError.ErrorMessage))
                    {
                        errors.Add(modelError.ErrorMessage);
                        continue;
                    }

                    if (modelError.Exception != null)
                        errors.Add(modelError.Exception.Message);
                }
            }
            return errors;
        }
    }
}