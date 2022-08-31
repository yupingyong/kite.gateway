using Kite.Gateway.Application;
using Kite.Gateway.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Kite.Gateway.Application.Contracts.Dtos;

namespace Kite.Gateway.Web.Controllers
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
        [HttpPost("/api/kite/refresh/configure")]
        public async Task<KiteResult> ReloadConfigureAsync(RefreshConfigureDto refreshConfigure)
        {
            return await _refreshAppService.RefreshConfigureAsync(refreshConfigure);
        }
    }
}
