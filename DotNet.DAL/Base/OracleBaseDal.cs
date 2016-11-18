using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using DotNet.Common.SqlHelper;
using DotNet.Entity.Enum;
using Oracle.DataAccess.Client;

namespace DotNet.DAL.Base
{
    public class OracleBaseDal<T> : BaseDal<T> where T : class ,new()
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
        public override DataTable GetTablePaged(int pageIndex, int pageSize, out int totalCount, string strWhere, Dictionary<string, SqlSortEnum> orderBy)
        {
            T entity = new T();
            string tableName = entity.GetType().ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from(");
            if (null != orderBy)//有Orderby条件
            {
                string strOrderBy = orderBy.Aggregate(string.Empty, (current, keyValuePair) => current + string.Format("{0} {1},", keyValuePair.Key, keyValuePair.Value));
                sb.AppendFormat("select *,ROW_NUMBER() over(order by {0}", strOrderBy.Substring(strOrderBy.Length - 1));
            }
            else
            {
                sb.AppendFormat("select *,ROW_NUMBER() over(order by {0} asc", GetPrimarykeyByTableName(tableName));
            }
            sb.AppendFormat(")  num from {0}", tableName);
            if (!string.IsNullOrEmpty(strWhere))//条件
            {
                sb.Append(" where @strWhere");
                totalCount = GetRecordCount(strWhere);
            }
            else
            {
                totalCount = GetRecordCount(string.Empty);
            }
            sb.AppendFormat(")  t where num between {0} and {1}", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
            IList<DbParameter> paramList = new List<DbParameter>() { new OracleParameter(":strWhere", strWhere) };
            return SqlHelperFactory.GetSqlHelper().ExecuteDataTable(sb.ToString(), paramList, CommandType.Text);
        }
        /// <summary>
        /// 获取表的主键
        /// </summary>
        /// <param name="tableName">要查询的表名</param>
        /// <returns>主键名:String</returns>
        public override string GetPrimarykeyByTableName(string tableName)
        {
           string commandText = "select column_name from all_cons_columns cc where owner='SCOTT' and table_name='" + tableName + "'and exists (select 'x' from all_constraints c where c.owner = cc.owner and c.constraint_name = cc.constraint_name and c.constraint_type ='P') order by position";
            return SqlHelperFactory.GetSqlHelper().ExecuteScalar(commandText, null).ToString();
        }
    }
}