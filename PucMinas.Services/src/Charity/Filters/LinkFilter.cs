using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PucMinas.Services.Charity.Domain.Results;

namespace PucMinas.Services.Charity.Filters
{
    public class LinkFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Controller is ControllerBase)
            {
                var controller = (ControllerBase) context.Controller;

                if (controller.Response.StatusCode >= 200 && controller.Response.StatusCode <= 299)
                {
                    var result = ((ObjectResult)context.Result)?.Value;

                    if (result != null && result.GetType().IsGenericType &&
                        result.GetType().GetGenericTypeDefinition() == typeof(PagedResponse<>))
                    {
                        var type = result.GetType();
                        var pagination = (Pagination) type.GetProperty("Pagination").GetValue(result);

                        if (pagination != null)
                        {
                            pagination.FormattLinks(context.HttpContext.Request);
                        }
                    }
                }
              
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
