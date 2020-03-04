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
        /// <param name="totalCount">总记录数</param>
        /// <returns></returns>
        public static IHtmlContent Pager(this IHtmlHelper htmlHelper, Http.HttpRequest request,int totalCount)
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
                int pageSize = 10;
                int pageCount = 0;
                //得到总页码数
                if (totalCount % pageSize == 0)
                {
                    pageCount = totalCount / pageSize;
                }
                else
                {
                    pageCount = totalCount / pageSize + 1;
                }

                //处理分页样式
                resultBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{url}\">回到首页</a></li>");

                resultBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{url}{(pageIndex == 1 ? "" :$"/{pageIndex - 1}" )}\">上一页</a></li>");
                //
                //中间页码计算
                int beginCount = 1;//开始页码
                if (pageIndex > 5)
                {
                    beginCount = pageIndex - 5;
                }
                int endCount = pageCount;
                if (pageCount - pageIndex > 5)
                {
                    endCount = pageIndex + 5;
                }

                for (int i = beginCount; i <= endCount; i++)
                {
                    if (pageIndex == i)
                    {
                        resultBuilder.Append($"<li class=\"page-item active\"><span class=\"page-link\">{i}</span></li>");
                    }
                    else
                    {
                        resultBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{url}/{i}\">{i}</a></li>");
                    }
                }
                //
                if (pageIndex < pageCount)
                {
                    resultBuilder.Append($"<li class=\"page-item\"><a class=\"page-link\" href=\"{url}/{pageIndex + 1}\">下一页</a></li>");
                }
                else
                {
                    resultBuilder.AppendFormat("<li class=\"page-item\"><a class=\"page-link active\" href=\"{0}\">你已经找到了最后一处藏数据的地方...</a></li>", "#");
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