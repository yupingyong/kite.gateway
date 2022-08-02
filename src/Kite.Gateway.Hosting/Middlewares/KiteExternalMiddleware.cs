using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Kite.Gateway.Domain.Shared.Options;
using Kite.Gateway.Domain.Shared.Enums;
using Kite.Gateway.Domain.Middlewares;
using System.Text;

namespace Kite.Gateway.Hosting.Middlewares
{
    public class KiteExternalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<MiddlewareOption> _middlewares;
        private readonly IHttpClientFactory _httpClientFactory;
        public KiteExternalMiddleware(RequestDelegate next
            , IOptions<List<MiddlewareOption>> options, IHttpClientFactory httpClientFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _middlewares = options.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            foreach (var middleware in _middlewares)
            {
                var middlewareResult = await HttpRequestAsync(middleware, context);
                if (middlewareResult.HttpStatusCode != 200)
                {
                    context.Response.StatusCode = middlewareResult.HttpStatusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(middlewareResult.Result, Encoding.UTF8);
                    return;
                }
            }
            await _next(context);
            return;
        }
        private async Task<MiddlewareResult> HttpRequestAsync(MiddlewareOption middleware, HttpContext context)
        {
            var middlewareResult = new MiddlewareResult()
            {
                HttpStatusCode = 0,
                Result = ""
            };
            if (middleware.SignalType == SignalTypeEnum.Http)
            {
                var httpClient = _httpClientFactory.CreateClient();
                //转发原请求头中带(K-)开头的请求头参数
                foreach (var header in context.Request.Headers.Where(x => x.Key.Contains("K-")).ToList())
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
                }
                foreach (var item in context.Items)
                {
                    httpClient.DefaultRequestHeaders.Add(Convert.ToString(item.Key), item.Value?.ToString());
                }
                var response = await httpClient.PostAsync(middleware.Server, null);
                var responseContent = await response.Content.ReadAsStringAsync();
                middlewareResult.Result = responseContent;
                middlewareResult.HttpStatusCode = (int)response.StatusCode;
            }
            return middlewareResult;
        }
    }
}
