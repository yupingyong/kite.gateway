using Kite.Gateway.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Kite.Gateway.Domain.Middlewares
{
    internal class MiddlewareManager : DomainService, IMiddlewareManager
    {
        private const string CacheKey = "AuthenticationConfigure";
        //
        private readonly IMemoryCache _memoryCache;
        private readonly IRepository<Middleware> _repository;

        public MiddlewareManager(IRepository<Middleware> repository, IMemoryCache memoryCache)
        {
            _repository = repository;
            _memoryCache = memoryCache;
        }
        public async Task<List<Middleware>> GetListAsync()
        {
            var data = _memoryCache.Get<List<Middleware>>(CacheKey);
            if (data == null)
            {
                data = await _repository.GetListAsync();
                if (data == null)
                {
                    throw new ArgumentNullException("身份认证信息未配置");
                }
                _memoryCache.Set(CacheKey, data, DateTimeOffset.Now.AddDays(7));
            }
            return data;
        }

        public async Task<Middleware> CreateAsync(string name, string server)
        {
            if (await _repository.AnyAsync(x => x.Name == name))
            {
                throw new ArgumentException("中间件名称不能重复");
            }
            if (await _repository.AnyAsync(x => x.Server == server))
            {
                throw new ArgumentException("中间件远程调用服务端地址不能重复");
            }
            return new Middleware(GuidGenerator.Create())
            {
                Created = DateTime.Now,
                Updated = DateTime.Now
            };
        }

        

        public async Task<Middleware> UpdateAsync(Guid id, string name, string server)
        {

            if (await _repository.AnyAsync(x => x.Name == name && x.Id != id))
            {
                throw new ArgumentException("中间件名称不能重复");
            }
            if (await _repository.AnyAsync(x => x.Server == server && x.Id != id))
            {
                throw new ArgumentException("中间件远程调用服务端地址不能重复");
            }
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new ArgumentNullException("中间件信息不存在");
            }
            model.Updated = DateTime.Now;
            return model;
        }
    }
}
