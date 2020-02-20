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
        public static bool MangoUpdate<TEntity>(this DbContext context, Expression<Func<TEntity, bool>> fields, Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
        {
            TSqlAssembledResult result = TSqlAssembled.Update<TEntity>(fields, predicate);
            context.Database.ExecuteSqlRaw(result.SqlStr);
            return context.SaveChanges() > 0 ? true : false;
        }
        /// <summary>
        /// 自定义更新扩展
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="entity">更新实体</param>
        /// <param name="predicate">更新条件</param>
        /// <returns></returns>
        public static bool MangoUpdate<TEntity>(this DbContext context, TEntity entity, Expression<Func<TEntity, bool>> predicate) where TEntity:class,new()
        {
            TSqlAssembledResult result = TSqlAssembled.Update<TEntity>(entity, predicate);
            context.Database.ExecuteSqlRaw(result.SqlStr, result.SqlParameters);
            return context.SaveChanges() > 0 ? true : false;
        }
        /// <summary>
        /// 自定义删除扩展
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="predicate">删除条件</param>
        /// <returns></returns>
        public static bool MangoRemove<TEntity>(this DbContext context,Expression<Func<TEntity, bool>> predicate) where TEntity : class,new()
        {
            TSqlAssembledResult result = TSqlAssembled.Delete<TEntity>(predicate);
            context.Database.ExecuteSqlRaw(result.SqlStr);
            return context.SaveChanges() > 0 ? true : false;
        }
        #endregion
        #region 查询返回Model的扩展
        /// <summary>
        /// SQL语句查询并且返回指定的实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<T> QueryModel<T>(this DbContext context, string sql, params SqlParameter[] parameters) where T : new()
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                command = connection.CreateCommand();
                command.CommandText = sql;
                command.Parameters.AddRange(parameters);
                DbDataReader reader = command.ExecuteReader();
                
                var result= FillModel<T>(reader);
                //释放连接资源
                command.Dispose();
                connection.Close();
                //
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //释放连接资源
                if (command != null)
                {
                    command.Dispose();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 将DbDataReader数据集转换成对应的实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_SqlDataReader"></param>
        /// <returns></returns>
        private static List<T> FillModel<T>(DbDataReader reader) where T : new()
        {
            List<T> list = new List<T>();
            while (reader.Read())
            {
                T m = new T();
                Type type = m.GetType();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    PropertyInfo pro = type.GetProperty(reader.GetName(i));
                    if (pro != null && reader.GetName(i).Length > 0)
                    {
                        object value = reader.GetValue(i);
                        if (value.GetType() != typeof(System.DBNull) && value != null)
                        {
                            pro.SetValue(m, DbIdentity.Change(value, pro.PropertyType), null);
                        }
                    }
                }
                list.Add(m);
            }
            return list;
        }
        #endregion
        #region 查询返回DataTable的扩展
        
        /// <summary>
        /// 查询数据并且返回DataTable
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sql">自定义SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public static DataTable QueryDataTable(this DbContext context, string sql, params SqlParameter[] parameters)
        {
            DbConnection connection = null;
            DbCommand command = null;
            try
            {
                connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                command = connection.CreateCommand();
                command.CommandText = sql;
                command.Parameters.AddRange(parameters);
                DbDataReader reader = command.ExecuteReader();
                var result= FillDataTable(reader);
                //释放连接资源
                command.Dispose();
                connection.Close();
                //
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //释放连接资源
                if (command != null)
                {
                    command.Dispose();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 转换成DataTable
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static DataTable FillDataTable(DbDataReader reader)
        {
            bool defined = false;

            DataTable table = new DataTable();
            
            while (reader.Read())
            {
                object[] values = new object[reader.FieldCount];
                //插入列信息
                if (!defined)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        DataColumn column = new DataColumn()
                        {
                            ColumnName = reader.GetName(i),
                            DataType = reader.GetFieldType(i)
                        };

                        table.Columns.Add(column);
                    }

                    defined = true;
                }
                //插入数据
                reader.GetValues(values);
                DataRow dataRow = table.NewRow();
                for (int i = 0; i < values.Length; i++)
                {
                    dataRow[i] = values[i];
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }
        #endregion
    }
}
