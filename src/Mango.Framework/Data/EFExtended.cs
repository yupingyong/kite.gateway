using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
namespace Mango.Framework.Data
{
    public static class EFExtended
    {
        #region 更新与删除扩展
        /// <summary>
        /// 指定更新数据列(修改指定的列)扩展
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static bool DesignationUpdate<TEntity>(this DbContext context, TEntity entity) where TEntity : class
        {
            context.Entry(entity).State = EntityState.Unchanged;
            //
            Type type = entity.GetType();
            //处理实体类属性
            PropertyInfo[] properties = type.GetProperties();
            foreach (var property in properties)
            {
                object value = property.GetValue(entity, null);
                var key = property.GetCustomAttribute<KeyAttribute>();
                if (value != null && key == null)
                {
                    context.Entry(entity).Property(property.Name).IsModified = true;
                }
            }
            return true;
        }
        /// <summary>
        /// 自定义更新扩展
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="fields">更新字段</param>
        /// <param name="predicate">更新条件</param>
        /// <returns></returns>
        public static int MangoUpdate<TEntity>(this DbContext context, Expression<Func<TEntity, bool>> fields, Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
        {
            TSqlAssembledResult result = TSqlAssembled.Update<TEntity>(fields, predicate);
            return context.Database.ExecuteSqlRaw(result.SqlStr);
        }
        /// <summary>
        /// 自定义更新扩展
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity">更新实体</param>
        /// <param name="predicate">更新条件</param>
        /// <returns></returns>
        public static int MangoUpdate<TEntity>(this DbContext context, TEntity entity, Expression<Func<TEntity, bool>> predicate) where TEntity:class,new()
        {
            TSqlAssembledResult result = TSqlAssembled.Update<TEntity>(entity, predicate);
            return context.Database.ExecuteSqlRaw(result.SqlStr, result.SqlParameters);
        }
        /// <summary>
        /// 自定义删除扩展
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="predicate">删除条件</param>
        /// <returns></returns>
        public static int MangoRemove<TEntity>(this DbContext context,Expression<Func<TEntity, bool>> predicate) where TEntity : class,new()
        {
            TSqlAssembledResult result = TSqlAssembled.Delete<TEntity>(predicate);
            return context.Database.ExecuteSqlRaw(result.SqlStr);
        }
        #endregion
    }
}
