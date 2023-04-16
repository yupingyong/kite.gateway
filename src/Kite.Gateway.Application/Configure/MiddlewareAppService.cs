using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Application.Contracts.Dtos.Middleware;
using Kite.Gateway.Domain.Entities;
using Kite.Gateway.Domain.Middlewares;
using Mapster;
using Volo.Abp.Domain.Repositories;

namespace Kite.Gateway.Application.Configure
{
    /// <summary>
    /// 中间件管理服务
    /// </summary>
    public class MiddlewareAppService : BaseApplicationService, IMiddlewareAppService
    {
        private readonly IMiddlewareManager _middlewareManager;
        private readonly IRepository<Middleware> _repository;
        public MiddlewareAppService(IMiddlewareManager middlewareManager, IRepository<Middleware> repository)
        {
            _middlewareManager = middlewareManager;
            _repository = repository;
        }

        public async Task<KiteResult> CreateAsync(CreateMiddlewareDto middlewareDto)
        {
            var model = await _middlewareManager.CreateAsync(middlewareDto.Name, middlewareDto.Server);
            middlewareDto.Adapt(model);
            await _repository.InsertAsync(model);
            return Ok();
        }

        public async Task<KiteResult> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(x => x.Id == id);
            return Ok();
        }

        public async Task<KiteResult<MiddlewareDto>> GetAsync(int id)
        {
            var result = (await _repository.GetQueryableAsync())
                .Where(x => x.Id == id)
                .ProjectToType<MiddlewareDto>()
                .FirstOrDefault();
            return Ok(result);
        }

        public async Task<KitePageResult<List<MiddlewareListDto>>> GetListAsync(string kw = "", int page = 1, int pageSize = 10)
        {
            var query = (await _repository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrEmpty(kw) && kw != "", x => x.Name.Contains(kw) || x.Server.Contains(kw));
            var totalCount = query.Count();
            var result = query
                .OrderByDescending(x => x.Created)
                .PageBy((page - 1) * pageSize, pageSize)
                .ProjectToType<MiddlewareListDto>()
                .ToList();
            return Ok(result, totalCount);
        }

        public async Task<KiteResult> UpdateAsync(UpdateMiddlewareDto middlewareDto)
        {
            var model = await _middlewareManager.UpdateAsync(middlewareDto.Id, middlewareDto.Name, middlewareDto.Server);
            middlewareDto.Adapt(model);
            model.Updated = DateTime.Now;
            await _repository.UpdateAsync(model);
            return Ok();
        }

        public async Task<KiteResult> UpdateUseStateAsync(int id, bool useState)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            model.UseState = useState;
            model.Updated = DateTime.Now;
            await _repository.UpdateAsync(model);
            return Ok();
        }
    }
}
