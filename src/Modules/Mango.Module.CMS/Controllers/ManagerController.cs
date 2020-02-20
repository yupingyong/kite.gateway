using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mango.Framework.Infrastructure;
using Mango.Framework.Data;
using Mango.Module.Core.Entity;
namespace Mango.Module.CMS.Controllers
{
    [Area("CMS")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public ManagerController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("{accountId}")]
        [HttpGet("{accountId}/{p}")]
        public IActionResult Get()
        {
            return APIReturnMethod.ReturnSuccess();
        }
    }
}