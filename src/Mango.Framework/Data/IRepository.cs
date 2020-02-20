using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Mango.Framework.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);
        /// <summary>
        /// 添加记录(批量)
        /// </summary>
        /// <param name="entity"></param>
        void Insert(IEnumerable<TEntity> entity);
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        /// <summary>
        /// 更新记录(批量)
        /// </summary>
        /// <param name="entity"></param>
        void Update(IEnumerable<TEntity> entity);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        /// 得到实体查询对象
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Query();

    }
}
