using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DotNet.DAL.Factory;
using DotNet.Entity.Enum;
using DotNet.IDAL;

namespace DotNet.DAL.Base
{
    public abstract class BaseDal<T>:IBaseDal<T> where T:class,new()
    {
        private static readonly string _dbMark = ConfigurationManager.AppSettings["dbMark"];
        private DbContext db;

        public BaseDal()
        {
            db = DbContextFactory.GetCurrentDbContext();
        }
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>受影响的行数</returns>
        public virtual int AdoAdd(T entity)
        {
            #region 参数
            IList<DbParameter> parameters = new List<DbParameter>();
            string fields = "";
            string placeholders = "";
            foreach (var item in entity.GetType().GetProperties())
            {
                if (item.Name == "Id")
                {
                    continue;
                }
                else
                {
                    fields += item.Name + ",";
                    placeholders += _dbMark + item.Name + ",";
                    DbParameter parameter = SqlHelperFactory.GetSqlHelper()
                        .CreateDbParameter(_dbMark + item.Name, item.GetValue(entity, null));
                    parameters.Add(parameter);
                }

            }
            #endregion
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("insert into " + entity.GetType().Name + " (");
            sqlStr.Append(fields.Substring(0, fields.Length - 1) + ")");
            sqlStr.Append(" values (");
            sqlStr.Append(placeholders.Substring(0, placeholders.Length - 1) + ")");
            //sqlStr.Append(";SELECT @@IDENTITY");   //返回插入用户的主键
            return SqlHelperFactory.GetSqlHelper().ExecuteNonQuery(sqlStr.ToString(), parameters);
        }

        /// <summary>
        /// 得到某列最大值,仅限数值
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <returns>整数值</returns>
        public virtual double AdoGetMax(string columnName)
        {
            double number;
            T entity = new T();
            IList<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(SqlHelperFactory.GetSqlHelper().CreateDbParameter(_dbMark + "columnName", columnName));
            string commandText = string.Format("select MAX(" + _dbMark + "columnName) from {0}", entity.GetType().Name);
            if (double.TryParse(SqlHelperFactory.GetSqlHelper().ExecuteScalar(commandText, parameters).ToString(), out number))//成功
            {
                return number;
            }
            else
            {
                return double.MinValue;
            }
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="primarykeyValue">主键值</param>
        /// <returns></returns>
        public virtual bool AdoExists(object primarykeyValue)
        {
            #region 参数
            T entity = new T();
            string primaryKeyName = AdoGetPrimarykeyByTableName(entity.GetType().Name);
            IList<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(SqlHelperFactory.GetSqlHelper().CreateDbParameter(_dbMark + primaryKeyName, primarykeyValue));
            #endregion
            string commandText = string.Format("select count(1) from {0} where {1}={2}{1}", entity.GetType().Name, primaryKeyName, _dbMark);
            DbDataReader reader = SqlHelperFactory.GetSqlHelper().ExecuteReader(commandText, parameters);
            if (reader.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>成功为True</returns>
        public virtual bool AdoUpdate(T entity)
        {
            #region 参数
            string primaryKeyName = AdoGetPrimarykeyByTableName(entity.GetType().Name);
            IList<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(SqlHelperFactory.GetSqlHelper().CreateDbParameter(_dbMark + primaryKeyName, entity.GetType().GetProperty(primaryKeyName).GetValue(entity, null)));
            string fields = "";
            foreach (var item in entity.GetType().GetProperties())
            {
                if (item.Name != primaryKeyName)
                {
                    fields += item.Name + "=" + _dbMark + item.Name + ",";
                    parameters.Add(SqlHelperFactory.GetSqlHelper().CreateDbParameter(_dbMark + item.Name, item.GetValue(entity, null)));
                }
            }
            #endregion
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("update " + entity.GetType().Name);
            sqlStr.Append(" set ");
            sqlStr.Append(fields.Substring(0, fields.Length - 1));
            sqlStr.Append(" where ");
            sqlStr.Append(primaryKeyName + "=" + _dbMark + primaryKeyName);
            int result = SqlHelperFactory.GetSqlHelper().ExecuteNonQuery(sqlStr.ToString(), parameters);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="primarykeyValue">主键值</param>
        /// <returns>成功为True</returns>
        public virtual bool AdoDelete(object primarykeyValue)
        {
            #region 参数
            T entity = new T();
            string primaryKeyName =AdoGetPrimarykeyByTableName(entity.GetType().Name.ToString());
            IList<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(SqlHelperFactory.GetSqlHelper().CreateDbParameter(_dbMark + primaryKeyName, primarykeyValue));
            #endregion
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("delete from {0} where ", entity.GetType().Name);
            sqlStr.Append(primaryKeyName + "=" + _dbMark + primaryKeyName);
            int res = SqlHelperFactory.GetSqlHelper().ExecuteNonQuery(sqlStr.ToString(), parameters);
            if (res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="primarykeyValues">主键值列表</param>
        /// <returns>成功为True</returns>
        public virtual bool AdoDeleteList(IList<object> primarykeyValues)
        {
            #region 参数
            T entity = new T();
            string primaryKeyName = AdoGetPrimarykeyByTableName(entity.GetType().Name);
            string primaryKeys = string.Empty;
            foreach (var item in primarykeyValues)
            {
                primaryKeys += item.ToString() + ",";
            }
            IList<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(SqlHelperFactory.GetSqlHelper().CreateDbParameter(_dbMark + primaryKeyName, primaryKeys.Substring(0, primaryKeys.Length - 1)));
            #endregion
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("delete from {0} where ", entity.GetType().Name);
            sqlStr.Append(primaryKeyName + " in(" + _dbMark + primaryKeyName + ")");
            int result = SqlHelperFactory.GetSqlHelper().ExecuteNonQuery(sqlStr.ToString(), parameters);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="primarykeyValue">主键值</param>
        /// <returns>返回查询到的对象实体,如果没有查询到则为null</returns>
        public virtual T AdoGetModel(object primarykeyValue)
        {
            #region 参数
            T entity = new T();
            string primaryKeyName = AdoGetPrimarykeyByTableName(entity.GetType().Name);
            IList<DbParameter> parameters = new List<DbParameter>();
            parameters.Add(SqlHelperFactory.GetSqlHelper().CreateDbParameter(_dbMark + primaryKeyName, primarykeyValue));
            #endregion
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.AppendFormat("select * from {0} where ", entity.GetType().Name);
            sqlStr.Append(primaryKeyName + "=" + _dbMark + primaryKeyName);
            return DataTableToList(SqlHelperFactory.GetSqlHelper().ExecuteDataTable(sqlStr.ToString(), parameters)).FirstOrDefault();
        }
        /// <summary>
        /// 获得指定数据
        /// </summary>
        /// <param name="strWhere">查询条件,若空则查询全部</param>
        /// <returns>数据表DataTable</returns>
        public virtual DataTable GetTable(string strWhere)
        {
            T entity = new T();
            string commandText = "select * from " + entity.GetType().Name;
            if (!string.IsNullOrEmpty(strWhere))
            {
                commandText += " where " + strWhere;
            }
            return SqlHelperFactory.GetSqlHelper().ExecuteDataTable(commandText, null);
        }

        /// <summary>
        /// 获得指定内容的记录数
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns>记录数</returns>
        public virtual int AdoGetRecordCount(string strWhere)
        {
            T entity = new T();
            string commandText = "select count(*) from " + entity.GetType().Name;
            if (!string.IsNullOrEmpty(strWhere))
            {
                commandText += " where " + strWhere;
            }
            return (int)SqlHelperFactory.GetSqlHelper().ExecuteScalar(commandText, null);
        }



        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="totalCount">总页数-output</param>
        /// <param name="strWhere">查询条件，若空则查询整个表</param>
        /// <param name="orderBy">排序依据的数据表列的名称</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns>返回DataTable数据表</returns>
        public abstract DataTable AdoGetTablePaged(int pageIndex, int pageSize, out int totalCount, string strWhere, Dictionary<string, SqlSortEnum> orderBy);
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>主键</returns>
        public virtual void ORMAdd(T entity)
        {
            db.Set<T>().Add(entity);
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        /// <returns>是否成功</returns>
        public virtual void ORMUpdate(T entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否成功</returns>
        public virtual void ORMDelete(T entity)
        {
            db.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// 通过主键删除一条记录
        /// </summary>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns>是否成功</returns>
        public virtual void ORMDeleteById(object primaryKeyValue)
        {
            T model = db.Set<T>().Find(new Guid(primaryKeyValue.ToString()));
            db.Entry(model).State = EntityState.Deleted;
        }
        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="entities">要删除对象实体的List集合</param>
        /// <returns>是否成功</returns>
        public virtual void ORMDeleteList(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                db.Set<T>().Remove(entity);//Delete(primarykey);
            }
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="whereLambda">查询记录的条件</param>
        /// <returns></returns>
        public virtual void ORMDeleteList(Expression<Func<T, bool>> whereLambda)
        {
            db.Set<T>().RemoveRange(db.Set<T>().Where(whereLambda));
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <returns>若存在返回True</returns>
        public virtual bool ORMExists(T entity)
        {
            return db.Entry(entity) == null ? true : false;
        }

        /// <summary>
        /// 返回一个对象实体
        /// </summary>
        /// <param name="primarykeyValue">主键值</param>
        /// <returns>对象实体</returns>
        public virtual T ORMGetModel(object primarykeyValue)
        {
            return db.Set<T>().Find(new Guid(primarykeyValue.ToString()));
        }
        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <param name="whereLambda">lambda查询表达式</param>
        /// <returns>IQueryable对象集合</returns>
        public virtual IQueryable<T> ORMLoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            //db.Set<UserInfo>().Where(u => u.ID > 10).Select(u => u);
            return db.Set<T>().Where(whereLambda).AsQueryable();
        }

        /// <summary>
        /// 分页查询实体集合
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="pageSize">页码</param>
        /// <param name="pageIndex">每页数据量</param>
        /// <param name="totalCount">总页数-out put</param>
        /// <param name="whereLambda">lambda查询表达式</param>
        /// <param name="isAsc">是否正序</param>
        /// <param name="orderBy">排序依据的字段</param>
        /// <returns>IQueryable集合</returns>
        public virtual IQueryable<T> ORMLoadPagedEntities<S>(int pageIndex, int pageSize, 
            out int totalCount,
            Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, S>> orderBy)
        {
            IQueryable<T> temp = db.Set<T>().Where(whereLambda).AsQueryable();
            totalCount = temp.Count();

            if (isAsc)
            {
                temp = temp.OrderBy(orderBy)
                           .Skip(pageSize * (pageIndex - 1))
                           .Take(pageSize).AsQueryable();
            }
            else
            {
                temp = temp.OrderByDescending(orderBy)
                          .Skip(pageSize * (pageIndex - 1))
                          .Take(pageSize).AsQueryable();
            }

            return temp;
        }

        public abstract string AdoGetPrimarykeyByTableName(string tableName);
        /// <summary>
        /// 获得集合实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dt">数据表</param>
        /// <returns>List集合</returns>
        private IEnumerable<T> DataTableToList(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            IList<T> list = new List<T>();
            T entity = default(T);
            foreach (DataRow dr in dt.Rows)
            {
                entity = Activator.CreateInstance<T>();
                PropertyInfo[] pis = entity.GetType().GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }
                        if (dr[pi.Name] != DBNull.Value)
                        {
                            Type t = pi.PropertyType;
                            if (t.FullName == "System.Guid")
                            {
                                pi.SetValue(entity, Guid.Parse(dr[pi.Name].ToString()), null);
                            }
                            else
                            {
                                pi.SetValue(entity, dr[pi.Name], null);
                            }

                        }
                    }
                }
                list.Add(entity);
            }
            return list;
        }
    }
}
