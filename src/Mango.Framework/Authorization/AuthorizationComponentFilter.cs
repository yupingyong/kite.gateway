using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mango.Framework.Authorization
{
    public class AuthorizationComponentFilter:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Action请求执行前处理事件
            //获取权限验证所需数据
            int roleId = 0;
            if (context.HttpContext.Session.GetInt32("RoleId") != null)
            {
                roleId = context.HttpContext.Session.GetInt32("RoleId").GetValueOrDefault(0);
            }
            string areaName = context.RouteData.Values["area"] != null ? context.RouteData.Values["area"].ToString().ToLower() : "";
            string controllerName = context.RouteData.Values["controller"].ToString().ToLower();
            string actionName = context.RouteData.Values["action"].ToString().ToLower();
            //获取授权组件
            var ac = Services.ServiceContext.GetService<IAuthorizationComponent>();
            var result= ac.Verification(roleId, areaName, controllerName, actionName);
            if (!result)
            {
                context.Result = new ContentResult()
                {
                    Content = "您的请求未得到授权!",
                    StatusCode = 401
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}
