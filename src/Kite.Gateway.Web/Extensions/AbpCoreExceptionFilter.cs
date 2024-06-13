using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Serilog;
using Microsoft.AspNetCore.Mvc;

namespace Kite.Gateway.Management.Web.Extensions
{
    public class AbpCoreExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Log.Error(context.Exception,context.Exception.Message);

            context.Result = new JsonResult(new KiteResult()
            {
                Code = 501,
                Message = context.Exception.Message
            });
            context.ExceptionHandled = true;
        }
    }
}
