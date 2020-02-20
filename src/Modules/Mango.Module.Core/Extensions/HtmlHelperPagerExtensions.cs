using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
namespace Microsoft.AspNetCore.Mvc.ViewFeatures
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// 自定义分页
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="rowCount">当前页记录条数</param>
        /// <returns></returns>
        public static IHtmlContent Pager(this IHtmlHelper htmlHelper, Http.HttpRequest request,int rowCount)
        {
            StringBuilder resultBuilder = new StringBuilder();//输出结果
            try
            {
                int pageIndex = 1;
                string url = request.Path;
                //获取当前页码
                if (request.RouteValues["p"] != null)
                {
                    pageIndex = Convert.ToInt32(request.RouteValues["p"].ToString());
                    url = url.Substring(0, url.LastIndexOf("/"));
                }
                //处理分页样式
                resultBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{url}\">回到首页</a></li>");

                resultBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{url}{(pageIndex == 1 ? "" :$"/{pageIndex - 1}" )}\">上一页</a></li>");

                resultBuilder.Append($"<li class=\"page-item active\"><span class=\"page-link\">{pageIndex}</span></li>");
                //
                if (rowCount > 0)
                {
                    resultBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{url}/{pageIndex + 1}\">下一页</a></li>");
                }
                else
                {
                    resultBuilder.AppendFormat("<li class=\"page-item\"><a class=\"page-link active\" href=\"{0}\">你已经到了一个没有任何数据的地方...</a></li>", "#");
                }
            }
            catch
            {
                resultBuilder.Append("分页出现异常...");
            }
            return new HtmlString(resultBuilder.ToString());
        }
    }
}