using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Kite.Gateway.Application;
using Serilog;
using Microsoft.AspNetCore.Mvc;

namespace Kite.Gateway.Hosting.Filters
{
    public class AbpCoreExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Log.Error(context.Exception,context.Exception.Message);

            context.Result = new JsonResult(new KiteResult() 
            {
                Code = 500,
                Message = context.Exception.Message
            });
            context.ExceptionHandled = true;
        }
    }
}
