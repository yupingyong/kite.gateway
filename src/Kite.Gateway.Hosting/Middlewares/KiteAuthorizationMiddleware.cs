using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Kite.Gateway.Domain.Shared.Options;
using System.Text.Json;
using System.Text.RegularExpressions;
using Kite.Gateway.Domain.Shared.Enums;
using Kite.Gateway.Domain.Authorization;

namespace Kite.Gateway.Hosting.Middlewares
{
    public class KiteAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<WhitelistOption> _whitelistOptions;
        private readonly AuthenticationOption _authenticationOption;
        private readonly IJwtTokenManager _jwtTokenManager;
        public KiteAuthorizationMiddleware(RequestDelegate next
            , IOptions<List<WhitelistOption>> whitelistOptions, IJwtTokenManager jwtTokenManager, AuthenticationOption authenticationOption)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _whitelistOptions = whitelistOptions.Value;
            _jwtTokenManager = jwtTokenManager;
            _authenticationOption = authenticationOption;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (!_authenticationOption.UseState)
            {
                await _next(context);
                return;
            }
            //白名单验证
            var requestPath = context.Request.Path.Value.ToLower();
            var reqeustMethod = context.Request.Method.ToUpper();
            bool IsWhitelist = false;
            foreach (var whitelist in _whitelistOptions)
            {
                switch (whitelist.FilterType)
                {
                    case FilterTypeEnum.Path:
                        if(requestPath.Contains(whitelist.FilterText.ToLower())&&reqeustMethod==whitelist.RequestMethod)
                            IsWhitelist=true;
                        break;
                    case FilterTypeEnum.Regular:
                        if(whitelist.Regex.IsMatch(requestPath) && reqeustMethod == whitelist.RequestMethod)
                            IsWhitelist = true;
                        break;
                }
                if (IsWhitelist)
                    break;
            }
            if (IsWhitelist)
            {
                await _next(context);
                return;
            }
            //token验证
            var tokenResult= await _jwtTokenManager.ValidationTokenAsync(context);
            if (!tokenResult.Successed)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(tokenResult.Message);
                return;
            }
            var claimTypes = new[] { "iat", "nbf", "exp", "iss", "aud" };
            var claims = tokenResult.Claims.Where(x => !claimTypes.Contains(x.Name)).ToList();
            foreach (var claim in claims)
            {
                if (!context.Request.Headers.ContainsKey(claim.Name))
                {
                    context.Request.Headers.Add($"K-{claim.Name}", System.Net.WebUtility.UrlEncode(claim.Value));
                }
            }
            await _next(context);
            return;
        }
    }
}
