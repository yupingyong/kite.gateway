using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mango.Framework.Infrastructure;
using Mango.Framework.Data;
using Mango.Module.Core.Entity;
using Newtonsoft.Json;

namespace Mango.Module.CMS.Areas.CMS.Controllers
{
    public class PlusController : Controller
    {
        private IUnitOfWork<MangoDbContext> _unitOfWork;
        public PlusController(IUnitOfWork<MangoDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public IActionResult Add()
        {
            return View();
        }
    }
}