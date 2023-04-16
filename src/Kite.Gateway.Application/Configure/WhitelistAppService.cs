using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Domain.Entities;
using Kite.Gateway.Domain.Whitelist;
using Kite.Gateway.Application.Contracts.Dtos.Whitelist;

namespace Kite.Gateway.Application.Configure
{
    /// <summary>
    /// 
    /// </summary>
    public class WhitelistAppService : BaseApplicationService, IWhitelistAppService
    {
        private readonly IWhiteListManager _whiteListManager;
        private readonly IRepository<Whitelist> _whiteListRepository;
        private readonly IRepository<Route> _routeRepository;
        public WhitelistAppService(
             IRepository<Whitelist> whiteListRepository, IWhiteListManager whiteListManager, IRepository<Route> routeListRepository)
        {
            _whiteListRepository = whiteListRepository;
            _whiteListManager = whiteListManager;
            _routeRepository = routeListRepository;
        }

        public async Task<KitePageResult<List<WhitelistDto>>> GetListAsync(string kw = "", int page = 1, int pageSize = 10)
        {
            var query = (await _whiteListRepository.GetQueryableAsync()).WhereIf(!string.IsNullOrEmpty(kw) && kw != "", x => x.Name.Contains(kw) || x.FilterText.Contains(kw));
            var totalCount = query.Count();
            var result = query
                .ProjectToType<WhitelistDto>()
                .OrderByDescending(x => x.Created)
                .PageBy((page - 1) * pageSize, pageSize).ToList();
            //获取对应路由信息
            var routes = await _routeRepository.GetListAsync();
            foreach (var item in result)
            {
                if (item.RouteId != 0)
                {
                    item.RouteName = routes.Where(x => x.Id == item.RouteId).Select(x => x.RouteName).FirstOrDefault();
                }
                else
                {
                    item.RouteName = "全局";
                }
            }
            return Ok(result, totalCount);
        }

        public async Task<KiteResult> DeleteAsync(int id)
        {
            var model = await _whiteListRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (model != null)
            {
                await _whiteListRepository.DeleteAsync(model);
            }
            return Ok();
        }

        public async Task<KiteResult> CreateAsync(CreateWhitelistDto createWhiteList)
        {
            var model = await _whiteListManager.CreateAsync(createWhiteList);
            await _whiteListRepository.InsertAsync(model);
            return Ok();
        }

        public async Task<KiteResult> UpdateAsync(UpdateWhitelistDto updateWhiteList)
        {
            var model = await _whiteListRepository.FirstOrDefaultAsync(x => x.Id == updateWhiteList.Id);
            if (model == null)
            {
                ThrownFailed("白名单信息不存在");
            }
            updateWhiteList.Adapt(model);
            await _whiteListRepository.UpdateAsync(model);
            return Ok();
        }

        public async Task<KiteResult<WhitelistDto>> GetAsync(int id)
        {
            var result = (await _whiteListRepository.GetQueryableAsync())
                .Where(x => x.Id == id)
                .ProjectToType<WhitelistDto>()
                .FirstOrDefault();
            return Ok(result);
        }

        public async Task<KiteResult> UpdateUseStateAsync(int id, bool useState)
        {
            var model = await _whiteListRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                ThrownFailed("白名单信息不存在");
            }
            model.UseState = useState;
            await _whiteListRepository.UpdateAsync(model);
            return Ok();
        }
    }
}
