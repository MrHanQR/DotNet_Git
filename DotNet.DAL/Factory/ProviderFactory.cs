using System;
using System.Collections.Generic;
using System.Data.Common;
using DotNet.Entity.Enum;

namespace DotNet.DAL.Factory
{
    public class ProviderFactory
    {
        private static Dictionary<DbProviderTypeEnum, string> providerInvariantNames = new Dictionary<DbProviderTypeEnum, string>();
        private static Dictionary<DbProviderTypeEnum, DbProviderFactory> providerFactoies = new Dictionary<DbProviderTypeEnum, DbProviderFactory>(20);
        static ProviderFactory()
        {
            //加载已知的数据库访问类的程序集  
            providerInvariantNames.Add(DbProviderTypeEnum.SqlServer, "System.Data.SqlClient");
            providerInvariantNames.Add(DbProviderTypeEnum.OleDb, "System.Data.OleDb");
            providerInvariantNames.Add(DbProviderTypeEnum.ODBC, "System.Data.ODBC");
            providerInvariantNames.Add(DbProviderTypeEnum.Oracle, "Oracle.DataAccess.Client");
            providerInvariantNames.Add(DbProviderTypeEnum.MySql, "MySql.Data.MySqlClient");
            providerInvariantNames.Add(DbProviderTypeEnum.SQLite, "System.Data.SQLite");
            providerInvariantNames.Add(DbProviderTypeEnum.Firebird, "FirebirdSql.Data.Firebird");
            providerInvariantNames.Add(DbProviderTypeEnum.PostgreSql, "Npgsql");
            providerInvariantNames.Add(DbProviderTypeEnum.DB2, "IBM.Data.DB2.iSeries");
            providerInvariantNames.Add(DbProviderTypeEnum.Informix, "IBM.Data.Informix");
            providerInvariantNames.Add(DbProviderTypeEnum.SqlServerCe, "System.Data.SqlServerCe");
        }
        /// <summary>  
        /// 获取指定数据库类型对应的程序集名称  
        /// </summary>  
        /// <param name="providerType">数据库类型枚举</param>  
        /// <returns></returns>  
        public static string GetProviderInvariantName(DbProviderTypeEnum providerType)
        {
            return providerInvariantNames[providerType];
        }
        /// <summary>  
        /// 获取指定类型的数据库对应的DbProviderFactory  
        /// </summary>  
        /// <param name="providerType">数据库类型枚举</param>  
        /// <returns></returns>  
        public static DbProviderFactory GetDbProviderFactory(DbProviderTypeEnum providerType)
        {
            //如果还没有加载，则加载该DbProviderFactory  
            if (!providerFactoies.ContainsKey(providerType))
            {
                providerFactoies.Add(providerType, ImportDbProviderFactory(providerType));
            }
            return providerFactoies[providerType];
        }
        /// <summary>  
        /// 加载指定数据库类型的DbProviderFactory  
        /// </summary>  
        /// <param name="providerType">数据库类型枚举</param>  
        /// <returns></returns>  
        private static DbProviderFactory ImportDbProviderFactory(DbProviderTypeEnum providerType)
        {
            string providerName = providerInvariantNames[providerType];
            DbProviderFactory factory = null;
            try
            {
                //从全局程序集中查找  
                factory = DbProviderFactories.GetFactory(providerName);
            }
            catch (ArgumentException ex)
            {
                factory = null;
                throw ex;
            }
            return factory;
        }
    }
}