using Kite.Gateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Domain.Repositories;
using Kite.Gateway.Domain.Shared;

namespace Kite.Gateway.Domain.Administrator
{
    internal class AdministratorManager : DomainService, IAdministratorManager
    {
        private readonly IRepository<Entities.Administrator> _repository;

        public AdministratorManager(IRepository<Entities.Administrator> repository)
        {
            _repository = repository;
        }
        public async Task<Entities.Administrator> LoginAsync(string adminName, string password)
        {
            
            //查询账号信息
            var administrator = await _repository.FirstOrDefaultAsync(x => x.AdminName == adminName && x.Password == password);
            if (administrator == null)
            {
                throw new ArgumentException("管理员账号或登录密码错误");
            }
            return administrator;
        }

        public async Task<Entities.Administrator> CreateAsync(string adminName)
        {
            if (await _repository.AnyAsync(x => x.AdminName == adminName))
            {
                throw new ArgumentException("管理员账号已经存在");
            }
            return new Entities.Administrator(GuidGenerator.Create())
            { 
                Created = DateTime.Now,
                Updated=DateTime.Now
            };
        }

        

        public async Task<Entities.Administrator> UpdateAsync(Guid id, string adminName)
        {
            if (await _repository.AnyAsync(x => x.AdminName == adminName && x.Id!=id))
            {
                throw new ArgumentException("管理员账号已经存在");
            }
            var administrator = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            administrator.Updated = DateTime.Now;
            return administrator;
        }
    }
}
