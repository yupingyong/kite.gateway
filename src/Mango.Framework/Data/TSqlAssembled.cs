using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
namespace Mango.Framework.Data
{
    /// <summary>
    /// T-Sql语句组装类 
    /// </summary>
    public static class TSqlAssembled
    {
        /// <summary>
        /// 更新语句组装
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fields"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TSqlAssembledResult Update<TEntity>(Expression<Func<TEntity, bool>> fields, Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
        {
            try
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("update ");
                strBuilder.Append(typeof(TEntity).Name);
                strBuilder.Append(" set ");
                //解析需要更新的字段值
                UpdateFieldBuilder updateFieldBuilder = new UpdateFieldBuilder();
                strBuilder.Append(updateFieldBuilder.Translate(fields));
                //解析条件
                ConditionBuilder conditionBuilder = new ConditionBuilder();
                strBuilder.Append(" where ");
                strBuilder.Append(conditionBuilder.Translate(predicate));
                //处理结果返回
                TSqlAssembledResult result = new TSqlAssembledResult();
                result.SqlParameters = null;
                result.SqlStr = strBuilder.ToString();
                return result;
            }
            catch(Exception ex)
            {
                return null;
                throw ex;
            }
        }
        /// <summary>
        /// 更新语句组装
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TSqlAssembledResult Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
        {
            try
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("update ");
                //
                Type type = entity.GetType();
                strBuilder.Append(type.Name);
                strBuilder.Append(" set ");
                //处理实体类属性
                PropertyInfo[] properties = type.GetProperties();
                int index = 0;
                List<SqlParameter> sqlParameter = new List<SqlParameter>();
                foreach (var property in properties)
                {
                    object value = property.GetValue(entity, null);
                    if (value != null)
                    {
                        if (index != 0)
                        {
                            strBuilder.Append(",");
                        }
                        strBuilder.Append(property.Name);
                        strBuilder.Append("=@");
                        strBuilder.Append(property.Name);

                        sqlParameter.Add(new SqlParameter(property.Name, value));
                        index++;
                    }
                }
                //编译条件
                ConditionBuilder conditionBuilder = new ConditionBuilder();
                strBuilder.Append(" where ");
                strBuilder.Append(conditionBuilder.Translate(predicate));
                //处理结果返回
                TSqlAssembledResult result = new TSqlAssembledResult();
                result.SqlParameters = sqlParameter.ToArray();
                result.SqlStr = strBuilder.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }
        /// <summary>
        /// 删除语句组装
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TSqlAssembledResult Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity:class,new()
        {
            try
            {
                string tableName = typeof(TEntity).Name;
                //条件编译
                ConditionBuilder conditionBuilder = new ConditionBuilder();
                string conditionStr = conditionBuilder.Translate(predicate);
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("delete from ");
                strBuilder.Append(tableName);
                strBuilder.Append(" where ");
                strBuilder.Append(conditionStr);
                //处理结果返回
                TSqlAssembledResult result = new TSqlAssembledResult();
                result.SqlParameters = null;
                result.SqlStr = strBuilder.ToString();
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
