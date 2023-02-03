using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Mapster;
using Kite.Gateway.Domain.Entities;

namespace Kite.Gateway.Domain.Whitelist
{
    internal class WhiteListManager : DomainService, IWhiteListManager
    {
        private readonly IRepository<Entities.Whitelist> _repository;
        public WhiteListManager(IRepository<Entities.Whitelist> repository)
        {
            _repository = repository;
        }
        public async Task<Entities.Whitelist> CreateAsync<T>(T whiteList)
        {
            var model= new Entities.Whitelist()
            {
                Created = DateTime.Now
            };
            TypeAdapter.Adapt(whiteList, model);
            if (await _repository.AnyAsync(x => x.RouteId == model.RouteId  && x.FilterText == model.FilterText))
            {
                throw new Exception("同一路由下已经存在相同的过滤文本值");
            }
            return model;
        }
        public Task<List<Entities.Whitelist>> GetListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
