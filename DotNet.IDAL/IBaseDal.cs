using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using DotNet.Entity.Enum;

namespace DotNet.IDAL
{
    public interface IBaseDal<T> where T : class, new()
    {
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>受影响的行数</returns>
        int AdoAdd(T entity);

        /// <summary>
        /// 得到某列最大值,仅限数字
        /// </summary>
        /// <param name="columnName">列明</param>
        /// <returns>整数值</returns>
        double AdoGetMax(string columnName);

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="primarykeyValue">主键值</param>
        /// <returns></returns>
        bool AdoExists(object primarykeyValue);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>成功为True</returns>
        bool AdoUpdate(T entity);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="primarykeyValue">主键值</param>
        /// <returns>成功为True</returns>
        bool AdoDelete(object primarykeyValue);

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="primarykeyValues">主键值列表</param>
        /// <returns>成功为True</returns>
        bool AdoDeleteList(IList<object> primarykeyValues);

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="primarykeyValue">主键值</param>
        /// <returns>返回查询到的对象实体,如果没有查询到则为null</returns>
        T AdoGetModel(object primarykeyValue);
        /// <summary>
        /// 获得指定数据
        /// </summary>
        /// <param name="strWhere">查询条件,若空则查询全部</param>
        /// <returns>数据表DataTable</returns>
        DataTable GetTable(string strWhere);
        /// <summary>
        /// 获得指定内容的记录数
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns>记录数</returns>
        int AdoGetRecordCount(string strWhere);
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
        DataTable AdoGetTablePaged(int pageIndex, int pageSize, out int totalCount, string strWhere, Dictionary<string, SqlSortEnum> orderBy);
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <returns>主键</returns>
        void ORMAdd(T entity);
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        /// <returns>是否成功</returns>
        void ORMUpdate(T entity);
        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否成功</returns>
        void ORMDelete(T entity);
        /// <summary>
        /// 通过主键删除一条记录
        /// </summary>
        /// <param name="primaryKeyValue">主键值</param>
        /// <returns>是否成功</returns>
        void ORMDeleteById(object primaryKeyValue);
        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="entities">要删除对象实体的List集合</param>
        /// <returns>是否成功</returns>
        void ORMDeleteList(IList<T> entities);
        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="whereLambda">查询记录的条件</param>
        /// <returns></returns>
        void ORMDeleteList(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="entity">对象实体</param>
        /// <returns>若存在返回True</returns>
        bool ORMExists(T entity);
        /// <summary>
        /// 返回一个对象实体
        /// </summary>
        /// <param name="primarykeyValue">主键</param>
        /// <returns>对象实体</returns>
        T ORMGetModel(object primarykeyValue);
        /// <summary>
        /// 查询实体集合
        /// </summary>
        /// <param name="whereLambda">lambda查询表达式</param>
        /// <returns>IQueryable对象集合</returns>
        IQueryable<T> ORMLoadEntities(Expression<Func<T, bool>> whereLambda);
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
        IQueryable<T> ORMLoadPagedEntities<S>(int pageIndex, int pageSize,
            out int totalCount,
           Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, S>> orderBy);

        //List<T> DataTableToList(DataTable dt);

        /// <summary>
        /// 获取表的主键
        /// </summary>
        /// <param name="tableName">要查询的表名</param>
        /// <returns></returns>
        string AdoGetPrimarykeyByTableName(string tableName);

        
    }
}