using Kite.Gateway.Application;
using Kite.Gateway.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Kite.Gateway.Application.Contracts.Dtos;
using Kite.Gateway.Application.Contracts.Dtos.Node;

namespace Kite.Gateway.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshController : ControllerBase
    {
        private readonly IRefreshAppService _refreshAppService;
        public RefreshController(IRefreshAppService refreshAppService)
        {
            _refreshAppService = refreshAppService;
        }
        [HttpGet("/api/kite/refresh/configure")]
        public async Task<KiteResult<RefreshConfigureDto>> GetConfigureAsync()
        {
            var result= await _refreshAppService.GetConfigureAsync(new ReloadConfigureDto() 
            {
                IsReloadAuthentication = true,
                IsReloadMiddleware = true,
                IsReloadServiceGovernance = true,
                IsReloadWhitelist = true,
                IsReloadYarp = true,
            });
            return result;
        }
    }
}
