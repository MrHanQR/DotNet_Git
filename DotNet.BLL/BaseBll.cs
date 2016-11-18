using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using DotNet.DALProvider;
using DotNet.Entity.Enum;
using DotNet.IBLL;
using DotNet.IDAL;

namespace DotNet.BLL
{
    public abstract class BaseBll<T> : IBaseBll<T> where T : class,new()
    {

        protected IDBSession DbSession;//当前类或者是子类
        //需要给CurrentDal赋值。基类 不知道当前Dal是谁。 子类知道。
        //逼迫子类给父类的一个属性赋值。
        protected IBaseDal<T> CurrentAdoDal;
        protected DbContext DbContext = DbContextFactory.GetCurrentDbContext();
        public BaseBll()
        {
            DbSession = DBSessionFactory.GetCurrentDbSession();
            SetCurrentDal();//构造函数里面调用了 抽象方法：SetCurrentDal
        }



        public abstract void SetCurrentDal();

        #region Ado.Net实现
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>受影响的行数</returns>
        public virtual int AdoAdd(T entity)
        {
            return CurrentAdoDal.Add(entity);
        }

        /// <summary>
        /// 得到某列最大值,仅限数字
        /// </summary>
        /// <param name="columnName">列明</param>
        /// <returns>整数值</returns>
        public virtual double AdoGetMax(string columnName)
        {
            return CurrentAdoDal.GetMax(columnName);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="primarykeyValue">主键值</param>
        /// <returns></returns>
        public virtual bool AdoExists(object primarykeyValue)
        {
            return CurrentAdoDal.Exists(primarykeyValue);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>成功为True</returns>
        public virtual bool AdoUpdate(T entity)
        {
            return CurrentAdoDal.Update(entity);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="primarykeyValue">主键值</param>
        /// <returns>成功为True</returns>
        public virtual bool AdoDelete(object primarykeyValue)
        {
            return CurrentAdoDal.Delete(primarykeyValue);
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="primarykeyValues">主键值列表</param>
        /// <returns>成功为True</returns>
        public virtual bool AdoDeleteList(IList<object> primarykeyValues)
        {
            return CurrentAdoDal.DeleteList(primarykeyValues);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="primarykeyValue">主键值</param>
        /// <returns>返回查询到的对象实体,如果没有查询到则为null</returns>
        public virtual T AdoGetModel(object primarykeyValue)
        {
            return CurrentAdoDal.GetModel(primarykeyValue);
        }

        /// <summary>
        /// 获得指定数据的List集合
        /// </summary>
        /// <param name="strWhere">查询条件,若空则查询全部</param>
        /// <returns>List数据集合</returns>
        public virtual IList<T> AdoGetList(string strWhere)
        {
            return DataTableToList(CurrentAdoDal.GetTable(strWhere));
        }

        /// <summary>
        /// 获取表中所有数据的List集合
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> AdoGetList()
        {
            return DataTableToList(CurrentAdoDal.GetTable(string.Empty));
        }

        /// <summary>
        /// 获得指定数据
        /// </summary>
        /// <param name="strWhere">查询条件,若空则查询全部</param>
        /// <returns>数据表DataTable</returns>
        public virtual DataTable GetTable(string strWhere)
        {
            return CurrentAdoDal.GetTable(strWhere);
        }
        /// <summary>
        /// 获得数据表
        /// </summary>
        /// <returns>数据表DataTable</returns>
        public virtual DataTable GetTable()
        {
            return CurrentAdoDal.GetTable(string.Empty);
        }

        /// <summary>
        /// 获得指定内容的记录数
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns>记录数</returns>
        public virtual int AdoGetRecordCount(string strWhere)
        {
            return CurrentAdoDal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="totalCount">总页数-output</param>
        /// <returns>返回DataTable数据表</returns>
        public virtual DataTable AdoGetTablePaged(int pageIndex, int pageSize, out int totalCount)
        {
            return CurrentAdoDal.GetTablePaged(pageIndex, pageSize, out totalCount, string.Empty, null);
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="totalCount">总页数-output</param>
        /// <param name="strWhere">查询条件，若空则查询整个表</param>
        /// <returns>返回DataTable数据表</returns>
        public virtual DataTable AdoGetTablePaged(int pageIndex, int pageSize, out int totalCount, string strWhere)
        {
            return CurrentAdoDal.GetTablePaged(pageIndex, pageSize, out totalCount, strWhere, null);
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
        public virtual DataTable AdoGetTablePaged(int pageIndex, int pageSize, out int totalCount, string strWhere, Dictionary<string, SqlSortEnum> orderBy)
        {
            return CurrentAdoDal.GetTablePaged(pageIndex, pageSize, out totalCount, strWhere, orderBy);
        }
        /// <summary>
        /// 分页获取数据的List集合
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="totalCount">总页数-output</param>
        /// <returns>返回实体的List集合</returns>
        public virtual IList<T> AdoGetListPaged(int pageIndex, int pageSize, out int totalCount)
        {
            return DataTableToList(AdoGetTablePaged(pageIndex, pageSize, out totalCount));
        }
        /// <summary>
        /// 分页获取数据的List集合
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="totalCount">总页数-output</param>
        /// <param name="strWhere">查询条件，若空则查询整个表</param>
        /// <returns>返回实体的List集合</returns>
        public virtual IList<T> AdoGetListPaged(int pageIndex, int pageSize, out int totalCount, string strWhere)
        {
            return DataTableToList(AdoGetTablePaged(pageIndex, pageSize, out totalCount, strWhere));
        }
        /// <summary>
        /// 分页获取数据的List集合
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="totalCount">总页数-output</param>
        /// <param name="strWhere">查询条件，若空则查询整个表</param>
        /// <param name="orderBy">排序依据的数据表列的名称</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns>返回实体的List集合</returns>
        public virtual IList<T> AdoGetListPaged(int pageIndex, int pageSize, out int totalCount, string strWhere, Dictionary<string, SqlSortEnum> orderBy)
        {
            return DataTableToList(AdoGetTablePaged(pageIndex, pageSize, out totalCount, strWhere, orderBy));
        } 
        #endregion

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>主键</returns>
        public virtual int Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
            return DbContext.SaveChanges();
        }
        /// <summary>
        /// 添加多条记录
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual int Add(IList<T> entities)
        {
            DbContext.Set<T>().AddRange(entities);
            return DbContext.SaveChanges();
        }
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        /// <returns>是否成功</returns>
        public virtual bool Update(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            return DbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否成功</returns>
        public virtual bool Delete(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
            return DbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 通过主键删除一条记录
        /// </summary>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns>是否成功</returns>
        public virtual bool Delete(object primaryKeyValue)
        {
            string primaryKey = primaryKeyValue.ToString();
            Guid id;
            if (Guid.TryParse(primaryKey,out id))
            {
                T model = DbContext.Set<T>().Find(id);
                DbContext.Entry(model).State = EntityState.Deleted;
                return DbContext.SaveChanges() > 0;
            }
            return false;
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="whereLambda">查询记录的条件</param>
        /// <returns></returns>
        public virtual int DeleteList(Expression<Func<T, bool>> whereLambda)
        {
            DbContext.Set<T>().RemoveRange(DbContext.Set<T>().Where(whereLambda));
            return DbContext.SaveChanges();
        }
        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="entities">要删除对象实体的List集合</param>
        /// <returns>是否成功</returns>
        public virtual bool DeleteList(IList<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);
            return DbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="idList">要删除对象实体的id的集合</param>
        /// <returns>是否成功</returns>
        public virtual bool DeleteList(string[] idList)
        {
            Guid id;
            for (int i = 0; i < idList.Length; i++)
            {

                if (Guid.TryParse(idList[i], out id))
                {
                    T model = DbContext.Set<T>().Find(id);
                    DbContext.Entry(model).State = EntityState.Deleted;
                }
                else
                {
                    return false;
                }
            }
            return DbContext.SaveChanges() > 0;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <returns>若存在返回True</returns>
        public virtual bool Exists(T entity)
        {
            return DbContext.Entry(entity)!=null;
        }

        /// <summary>
        /// 返回一个对象实体
        /// </summary>
        /// <param name="primarykeyValue">主键</param>
        /// <returns>对象实体</returns>
        public virtual T GetModel(object primarykeyValue)
        {
            Guid id;
            if (Guid.TryParse(primarykeyValue.ToString(),out id))
            {
                return DbContext.Set<T>().Find(id);
            }
            return null;
            
        }
        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <param name="whereLambda">lambda查询表达式</param>
        /// <returns>IQueryable对象集合</returns>
        public virtual IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return DbContext.Set<T>().Where(whereLambda);
        }
        public int GetRecordTotalCount(Expression<Func<T, bool>> whereLambda)
        {
            return DbContext.Set<T>().Where(whereLambda).Count();
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
        public virtual IQueryable<T> LoadPagedEntities<S>(int pageIndex, int pageSize,
            out int totalCount,
            Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, S>> orderBy)
        {
            totalCount = GetRecordTotalCount(whereLambda);
            if (totalCount<1)
            {
                return null;
            }
            else
            {
                IQueryable<T> temp = DbContext.Set<T>().Where(whereLambda).AsQueryable();
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
            //return CurrentAdoDal.ORMLoadPagedEntities(pageSize, pageIndex, out totalCount, whereLambda, isAsc, orderBy);
        }
        /// <summary>
        /// 获得集合实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="dt">数据表</param>
        /// <returns>List集合</returns>
        public virtual IList<T> DataTableToList(DataTable dt)
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
       
        ///// <summary>
        ///// 将IEnumerable<T>类型的集合转换为DataTable类型 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="varlist"></param>
        ///// <returns></returns>

        //public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        //{   //定义要返回的DataTable对象
        //    DataTable dtReturn = new DataTable();
        //    // 保存列集合的属性信息数组
        //    PropertyInfo[] oProps = null;
        //    if (varlist == null) return dtReturn;//安全性检查
        //    //循环遍历集合，使用反射获取类型的属性信息
        //    foreach (T rec in varlist)
        //    {
        //        //使用反射获取T类型的属性信息，返回一个PropertyInfo类型的集合
        //        if (oProps == null)
        //        {
        //            oProps = ((Type)rec.GetType()).GetProperties();
        //            //循环PropertyInfo数组
        //            foreach (PropertyInfo pi in oProps)
        //            {
        //                Type colType = pi.PropertyType;//得到属性的类型
        //                //如果属性为泛型类型
        //                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
        //                == typeof(Nullable<>)))
        //                {   //获取泛型类型的参数
        //                    colType = colType.GetGenericArguments()[0];
        //                }
        //                //将类型的属性名称与属性类型作为DataTable的列数据
        //                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
        //            }
        //        }
        //        //新建一个用于添加到DataTable中的DataRow对象
        //        DataRow dr = dtReturn.NewRow();
        //        //循环遍历属性集合
        //        foreach (PropertyInfo pi in oProps)
        //        {   //为DataRow中的指定列赋值
        //            dr[pi.Name] = pi.GetValue(rec, null) == null ?
        //                DBNull.Value : pi.GetValue(rec, null);
        //        }
        //        //将具有结果值的DataRow添加到DataTable集合中
        //        dtReturn.Rows.Add(dr);
        //    }
        //    return dtReturn;//返回DataTable对象
        //}



 
    }
}