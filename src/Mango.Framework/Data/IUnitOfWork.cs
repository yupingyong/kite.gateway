using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace Mango.Framework.Data
{

    /// <summary>
    /// 
    /// </summary>
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        /// <summary>
        /// EF Core上下文
        /// </summary>
        TContext DbContext { get; }
        /// <summary>
        /// 获取指定TEntity的仓储对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase;
        /// <summary>
        /// 持久化到数据库(同步)
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// 持久化到数据库(异步)
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        int Commit();
    }
}
