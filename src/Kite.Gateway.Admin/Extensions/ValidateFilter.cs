using Kite.Gateway.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Kite.Gateway.Admin.Extensions
{
    public class ValidateFilter : IActionFilter, ITransientDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                         .Where(e1 => e1.Value.Errors.Count > 0)
                         .Select(e1 => e1.Value.Errors.First().ErrorMessage)
                         .ToList();
                var errorResult = string.Join("|", errors);
                context.Result = new JsonResult(new KiteResult()
                {
                    Code =502,
                    Message = errorResult
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
