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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public Repository(MangoDbContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        protected DbContext Context { get; }

        protected DbSet<TEntity> DbSet { get; }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }
        /// <summary>
        /// 添加记录(批量)
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(IEnumerable<TEntity> entity)
        {
            DbSet.AddRange(entity);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
        /// <summary>
        /// 更新记录(批量)
        /// </summary>
        /// <param name="entity"></param>
        public void Update(IEnumerable<TEntity> entity)
        {
            DbSet.UpdateRange(entity);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }
        /// <summary>
        /// 得到实体查询对象
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Query()
        {
            return DbSet;
        }

        
    }
}
