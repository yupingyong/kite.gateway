using Kite.Gateway.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
namespace Kite.Gateway.Hosting.Filters
{
    public class KiteCoreActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
            if (configuration != null)
            {
                var accessToken = configuration.GetSection("AccessToken").Value;
                if (!context.HttpContext.Request.Headers.Where(x => x.Key == "AccessToken").Any())
                {
                    context.Result = new ContentResult()
                    {
                        Content = "授权访问Token为空",
                        StatusCode = 401
                    };
                }
                if (context.HttpContext.Request.Headers["AccessToken"].ToString() != accessToken)
                {
                    context.Result = new ContentResult()
                    {
                        Content = "授权访问Token错误",
                        StatusCode = 401
                    };
                }
            }
        }
    }
}
