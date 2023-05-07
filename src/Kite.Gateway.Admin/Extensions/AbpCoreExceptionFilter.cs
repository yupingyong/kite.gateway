using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Kite.Gateway.Application;

namespace Kite.Gateway.Admin.Extensions
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
