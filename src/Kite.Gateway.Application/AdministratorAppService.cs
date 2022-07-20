using Kite.Gateway.Application.Contracts;
using Kite.Gateway.Application.Contracts.Dtos.Administrator;
using Kite.Gateway.Domain.Administrator;
using Kite.Gateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Mapster;
using Kite.Gateway.Domain.Shared;

namespace Kite.Gateway.Application
{
    public class AdministratorAppService : BaseApplicationService, IAdministratorAppService
    {
        private readonly IRepository<Administrator> _repository;
        private readonly IAdministratorManager _administratorManager;

        public AdministratorAppService(IAdministratorManager administratorManager, IRepository<Administrator> repository)
        {
            _administratorManager = administratorManager;
            _repository = repository;
        }
        public async Task<HttpResponseResult<AdministratorDto>> LoginAsync(LoginAdministratorDto loginAdministrator)
        {
            //如果账号未初始化则默认初始化一个账号
            if (await _repository.CountAsync() <= 0)
            {
                await _repository.InsertAsync(new Administrator(GuidGenerator.Create())
                {
                    AdminName = "admin",
                    Password = TextHelper.MD5Encrypt("admin"),
                    Created = DateTime.Now,
                    NickName = "默认管理员",
                    Updated = DateTime.Now
                });
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            var administrator =await _administratorManager.LoginAsync(loginAdministrator.AdminName, TextHelper.MD5Encrypt(loginAdministrator.Password));
            var result = TypeAdapter.Adapt<AdministratorDto>(administrator);
            return Ok(result);
        }

        public async Task<HttpResponseResult> CreateAsync(CreateAdministratorDto createAdministrator)
        {
            var model =await _administratorManager.CreateAsync(createAdministrator.AdminName);
            model.AdminName = createAdministrator.AdminName;
            model.NickName = createAdministrator.NickName;
            model.Password = TextHelper.MD5Encrypt(createAdministrator.Password);
            await _repository.InsertAsync(model);
            return Ok();
        }

        public async Task<HttpResponseResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(x => x.Id == id);
            return Ok();
        }

        public async Task<HttpResponseResult<AdministratorDto>> GetAsync(Guid id)
        {
            var result = (await _repository.GetQueryableAsync())
                .Where(x => x.Id == id)
                .ProjectToType<AdministratorDto>()
                .FirstOrDefault();
            return Ok(result);
        }

        public async Task<HttpResponsePageResult<List<AdministratorDto>>> GetListAsync(int page = 1, int pageSize = 10)
        {
            var query = (await _repository.GetQueryableAsync());
            var result = query.OrderByDescending(x => x.Created)
                .PageBy((page - 1) * pageSize, pageSize)
                .ProjectToType<AdministratorDto>()
                .ToList();
            return Ok(result, query.Count());
        }

        

        public async Task<HttpResponseResult> UpdateAsync(UpdateAdministratorDto updateAdministrator)
        {
            var model = await _administratorManager.UpdateAsync(updateAdministrator.Id, updateAdministrator.AdminName);
            model.AdminName = updateAdministrator.AdminName;
            model.NickName = updateAdministrator.NickName;
            model.Updated = DateTime.Now;
            if (!string.IsNullOrEmpty(updateAdministrator.Password) && updateAdministrator.Password != "")
            {
                model.Password = TextHelper.MD5Encrypt(updateAdministrator.Password);
            }
            await _repository.UpdateAsync(model);
            return Ok();
        }
    }
}
