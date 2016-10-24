using System;
using System.Configuration;
using System.Web;
using DotNet.DAL.Base;
using DotNet.Entity.Enum;

namespace DotNet.DAL.Factory
{
    public class SqlHelperFactory
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        private static readonly string DbType = ConfigurationManager.AppSettings["dbTpye"];
        public SqlHelperFactory()
        {}

        public static SqlHelper GetSqlHelper()
        {
            //线程内实例唯一
            var db = HttpContext.Current.Items["SqlHelper"] as SqlHelper;
            if (db == null)
            {
                db = new SqlHelper(ConnectionString, (DbProviderTypeEnum)Enum.Parse(typeof(DbProviderTypeEnum), DbType, true));
                HttpContext.Current.Items.Add("SqlHelper", db);
            }
            return db;
        }
    }
}