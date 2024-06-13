using Kite.Gateway.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Mapster;
using Kite.Gateway.Application.Dtos.Auth;
using Kite.Gateway.Domain.Shared.Infrastructure;
using Kite.Gateway.Application;
namespace Kite.Gateway.Application.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthAccountAppService:BaseApplicationService
    {
        private readonly IRepository<AuthAccount> _repository;
        private readonly IRepository<AuthRole> _roleRepository;

        public AuthAccountAppService(IRepository<AuthRole> roleRepository, IRepository<AuthAccount> repository)
        {
            _roleRepository = roleRepository;
            _repository = repository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountStateDto"></param>
        /// <returns></returns>
        public async Task<KiteResult> PutStateAsync(AuthAccountStateDto accountStateDto)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == accountStateDto.Id);
            model.Updated = DateTime.Now;
            model.IsEnable = accountStateDto.IsEnable;
            await _repository.UpdateAsync(model);
            return Ok();
        }
        /// <summary>
        /// 账号编辑/新增
        /// </summary>
        /// <param name="accountEditDto"></param>
        /// <returns></returns>
        public async Task<KiteResult> PostEditAsync(AuthAccountEditDto accountEditDto)
        {
            AuthAccount model;
            if (accountEditDto.Id == Guid.Empty)
            {
                model = new AuthAccount(GuidGenerator.Create())
                {
                    AccountName = accountEditDto.AccountName,
                    Created = DateTime.Now,
                    HeadUrl = accountEditDto.HeadUrl,
                    IsEnable = accountEditDto.IsEnable,
                    NickName = accountEditDto.NickName,
                    Password = string.IsNullOrWhiteSpace(accountEditDto.Password) ? TextHelper.MD5Encrypt("123456") : TextHelper.MD5Encrypt(accountEditDto.Password),
                    RoleId = accountEditDto.RoleId,
                    Updated = DateTime.Now
                };
                await _repository.InsertAsync(model);
            }
            else 
            {
                model = await _repository.FirstOrDefaultAsync(x => x.Id == accountEditDto.Id);
                model.AccountName = accountEditDto.AccountName;
                model.Updated = DateTime.Now;
                model.NickName = accountEditDto.NickName;
                if (!string.IsNullOrWhiteSpace(accountEditDto.Password))
                {
                    model.Password = TextHelper.MD5Encrypt(accountEditDto.Password);
                }
                model.HeadUrl = accountEditDto.HeadUrl;
                model.RoleId = accountEditDto.RoleId;
                model.IsEnable = accountEditDto.IsEnable;
                await _repository.UpdateAsync(model);
            }
            return Ok();
        }
        /// <summary>
        /// 删除管理员账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<KiteResult> DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(x=>x.Id==id);
            return Ok();
        }
        /// <summary>
        /// 获取管理员账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<KiteResult<AuthAccountDto>> GetAsync(Guid id)
        {
            var result=(await _repository.GetQueryableAsync())
                .Where(x=>x.Id == id)
                .ProjectToType<AuthAccountDto>()
                .FirstOrDefault();
            return Ok(result);
        }
        /// <summary>
        /// 获取管理员账号列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<KitePageResult<List<AuthAccountDto>>> GetPageAsync(int page=1,int limit=10)
        {
            var query = (await _repository.GetQueryableAsync())
                .Join(await _roleRepository.GetQueryableAsync(),x=>x.RoleId,y=>y.Id,(x,y)=>new AuthAccountDto()
                {
                    AccountName = x.AccountName,
                    Created=x.Created,
                    HeadUrl = x.HeadUrl,
                    Id = x.Id,
                    IsEnable = x.IsEnable,
                    NickName=x.NickName,
                    RoleId=x.RoleId,
                    RoleName=y.RoleName,
                    Updated=x.Updated
                });
            var count = query.Count();
            var result = query.OrderByDescending(x => x.Created)
                .PageBy((page - 1) * limit, limit)
                .ToList();
            return Ok(result, count);
        }
    }
}
