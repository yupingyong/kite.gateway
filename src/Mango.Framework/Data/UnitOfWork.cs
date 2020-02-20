using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
namespace Mango.Framework.Data
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : MangoDbContext
    {
        private readonly TContext _context;
        private bool _disposed = false;
        private TransactionScope _scope;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        /// <summary>
        /// EF Core上下文
        /// </summary>
        public TContext DbContext => _context;
        /// <summary>
        /// 获取指定TEntity的仓储对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
        {
            var customRepo = _context.GetService<IRepository<TEntity>>();
            if (customRepo != null)
            {
                return customRepo;
            }
            return new Repository<TEntity>(_context);
        }
        /// <summary>
        /// 持久化到数据库(同步)
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
           return _context.SaveChanges();
        }
        /// <summary>
        /// 持久化到数据库(异步)
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            if (_scope == null)
            {
                _scope = new TransactionScope();
            }
        }
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            try
            {
                int count = this.SaveChanges();
                _scope.Complete();
                return count;
            }
            catch
            {
                return 0;
            }
            finally
            {
                _scope.Dispose();
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
