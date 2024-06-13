using Kite.Gateway.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Kite.Gateway.Application.Dtos.Auth;
using Mapster;

namespace Kite.Gateway.Application.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthMenuAppService:BaseApplicationService
    {
        private readonly IRepository<AuthMenu> _repository;

        public AuthMenuAppService(IRepository<AuthMenu> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 编辑/新增菜单信息
        /// </summary>
        /// <param name="menuEditDto"></param>
        /// <returns></returns>
        public async Task<KiteResult> PostEditAsync(AuthMenuEditDto menuEditDto)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == menuEditDto.Id);
            if (model == null)
            {
                if (await _repository.AnyAsync(x => x.AuthCode == menuEditDto.AuthCode))
                {
                    return Fail("授权码已经存在,不能重复添加");
                }
                model = new AuthMenu(GuidGenerator.Create())
                {
                    Created=DateTime.Now,
                    Updated=DateTime.Now
                };
                TypeAdapter.Adapt(menuEditDto, model);
                await _repository.InsertAsync(model);
            }
            else
            {
                if (await _repository.AnyAsync(x => x.AuthCode == menuEditDto.AuthCode && x.Id != menuEditDto.Id))
                {
                    return Fail("授权码已经存在,不能重复添加");
                }
                model.Updated=DateTime.Now;
                TypeAdapter.Adapt(menuEditDto, model);
                await _repository.UpdateAsync(model);
            }
            return Ok();
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<KiteResult> DeleteAsync(Guid id)
        {
            if (await _repository.AnyAsync(x => x.ParentId == id))
            {
                return Fail("该菜单下包含子菜单不能进行删除！");
            }
            await _repository.DeleteAsync(x=>x.Id==id);
            return Ok();    
        }
        /// <summary>
        /// 根据ID获取菜单信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<KiteResult<AuthMenuDto>> GetAsync(Guid id)
        {
            var result = (await _repository.GetQueryableAsync())
                .Where(x=>x.Id==id)
                .ProjectToType<AuthMenuDto>()
                .FirstOrDefault();
            return Ok(result);
        }
        /// <summary>
        /// 获取一级菜单
        /// </summary>
        /// <returns></returns>
        public async Task<KiteResult<List<AuthMenuDto>>> GetOneAsync()
        {
            var result = (await _repository.GetQueryableAsync())
                .Where(x => x.ParentId == Guid.Empty)
                .OrderBy(x => x.Created)
                .ProjectToType<AuthMenuDto>()
                .ToList();
            return Ok(result);
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public async Task<KiteResult<List<AuthMenuTreeDto>>> GetTreeAsync()
        {
            var menus = (await _repository.GetQueryableAsync())
                .OrderBy(x => x.Created)
                .ProjectToType<AuthMenuTreeDto>()
                .ToList();
            var result = menus.Where(x => x.ParentId == Guid.Empty).ToList();
            foreach (var item in result) 
            {
                item.IsParent = true;
                item.Children = menus.Where(x => x.ParentId == item.Id).ToList();
            }
            return Ok(result);
        }
    }
}
