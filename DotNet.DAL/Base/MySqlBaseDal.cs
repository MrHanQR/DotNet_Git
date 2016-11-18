using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DotNet.Common.SqlHelper;
using DotNet.Entity.Enum;

namespace DotNet.DAL.Base
{
    public class MySqlBaseDal<T>:BaseDal<T> where T:class,new()
    {
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalCount">总页数-out</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderBy">《列名,ASC/DESC》</param>
        /// <returns>DataTable</returns>
        public override DataTable GetTablePaged(int pageIndex, int pageSize, out int totalCount, string strWhere, Dictionary<string,SqlSortEnum> orderBy )
        {
            //改写 order by is Asc,多条件排序
            //string sql = "select * from user where (1=1 and host='localhost') order by host desc limit 1,4";跟SQLServer不一样
            //MySql参数化怎么写？以后再写
            T entity = new T();
            string tableName = entity.GetType().ToString();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from {0}", tableName);
            if (!string.IsNullOrEmpty(strWhere))//有where条件
            {
                sb.Append(" where ( ").Append(strWhere).Append(" )");
                totalCount = GetRecordCount(strWhere);
            }
            else
            {
                totalCount = GetRecordCount(string.Empty);
            }
            if (null!=orderBy)//有Orderby条件
            {
                string strOrderBy = orderBy.Aggregate(string.Empty, (current, keyValuePair) => current + string.Format("{0} {1},", keyValuePair.Key, keyValuePair.Value));
                //string strOrderBy = string.Empty;
                //foreach (KeyValuePair<string, SqlSortEnum> keyValuePair in orderBy)
                //{
                //    strOrderBy += string.Format("{0} {1},", keyValuePair.Key, keyValuePair.Value);
                //}
                sb.AppendFormat("order by {0}", strOrderBy.Substring(0,strOrderBy.Length-1));
            }
            else
            {
                sb.AppendFormat("order by {0} asc", GetPrimarykeyByTableName(tableName));
            }
            sb.AppendFormat(" limit {0},{1}", (pageIndex - 1)*pageSize, pageSize);
            //List<DbParameter> paramList = new List<DbParameter>() { new MySqlParameter("@strWhere", strWhere) };
            return SqlHelperFactory.GetSqlHelper().ExecuteDataTable(sb.ToString(), null, CommandType.Text);
        }
        /// <summary>
        /// 获取表的主键
        /// </summary>
        /// <param name="tableName">要查询的表名</param>
        /// <returns>主键名:String</returns>
        public override string GetPrimarykeyByTableName(string tableName)
        {
           string commandText = "select COLUMN_KEY,COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where table_name='" +
                                 tableName + "' AND COLUMN_KEY='PRI'";
           return SqlHelperFactory.GetSqlHelper().ExecuteDataTable(commandText, null).Rows[0]["COLUMN_NAME"].ToString();
        }
    }
}