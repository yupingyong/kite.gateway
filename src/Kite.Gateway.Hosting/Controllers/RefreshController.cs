using Kite.Gateway.Application;
using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Application.Contracts.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace Kite.Gateway.Hosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshController : ControllerBase
    {
        private readonly IRefreshAppService _refreshAppService;
        private readonly IConfiguration _configuration;
        public RefreshController(IRefreshAppService refreshAppService, IConfiguration configuration)
        {
            _refreshAppService = refreshAppService;
            _configuration = configuration;
        }
        [HttpPost("/api/kite/refresh/configure")]
        public async Task<HttpResponseResult> ReloadConfigureAsync(ReloadConfigureDto reloadConfigure)
        {
            
            return await _refreshAppService.ReloadConfigureAsync(reloadConfigure);
        }
    }
}
