using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DotNet.DAL.Factory;
using DotNet.Entity.Enum;

namespace DotNet.DAL.Base
{
    public class SqlHelper
    {
        private string _connectionString { get; set; }
        private DbProviderFactory providerFactory;
        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="connectionString">数据库连接字符串</param>  
        /// <param name="providerType">数据库类型枚举，参见<paramref name="providerType"/></param>  
        public SqlHelper(string connectionString, DbProviderTypeEnum providerType)
        {
            _connectionString = connectionString;
            providerFactory = ProviderFactory.GetDbProviderFactory(providerType);
            if (providerFactory == null)
            {
                throw new ArgumentException("Can't load DbProviderFactory for given value of providerType");
            }
        }
        /// <summary>     
        /// 对数据库执行增删改操作，返回受影响的行数。     
        /// </summary>     
        /// <param name="sql">要执行的增删改的SQL语句</param>     
        /// <param name="parameters">执行增删改语句所需要的参数</param>  
        /// <returns></returns>    
        public int ExecuteNonQuery(string sql, IList<DbParameter> parameters)
        {
            return ExecuteNonQuery(sql, parameters, CommandType.Text);
        }
        /// <summary>     
        /// 对数据库执行增删改操作，返回受影响的行数。     
        /// </summary>     
        /// <param name="sql">要执行的增删改的SQL语句</param>     
        /// <param name="parameters">执行增删改语句所需要的参数</param>  
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <returns></returns>  
        public int ExecuteNonQuery(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(sql, parameters, commandType))
            {
                command.Connection.Open();
                int affectedRows = command.ExecuteNonQuery();
                command.Connection.Close();
                return affectedRows;
            }
        }

        /// <summary>     
        /// 执行一个查询语句，返回一个关联的DataReader实例     
        /// </summary>     
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>  
        /// <returns></returns>   
        public DbDataReader ExecuteReader(string sql, IList<DbParameter> parameters)
        {
            return ExecuteReader(sql, parameters, CommandType.Text);
        }

        /// <summary>     
        /// 执行一个查询语句，返回一个关联的DataReader实例     
        /// </summary>     
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>  
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <returns></returns>   
        public DbDataReader ExecuteReader(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            DbCommand command = CreateDbCommand(sql, parameters, commandType);
            command.Connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>     
        /// 执行一个查询语句，返回一个包含查询结果的DataTable     
        /// </summary>     
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>  
        /// <returns></returns>  
        public DataTable ExecuteDataTable(string sql, IList<DbParameter> parameters)
        {
            return ExecuteDataTable(sql, parameters, CommandType.Text);
        }
        /// <summary>     
        /// 执行一个查询语句，返回一个包含查询结果的DataTable     
        /// </summary>     
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>  
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <returns></returns>  
        public DataTable ExecuteDataTable(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(sql, parameters, commandType))
            {
                using (DbDataAdapter adapter = providerFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }

        /// <summary>     
        /// 执行一个查询语句，返回查询结果的第一行第一列     
        /// </summary>     
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>     
        /// <returns></returns>     
        public Object ExecuteScalar(string sql, IList<DbParameter> parameters)
        {
            return ExecuteScalar(sql, parameters, CommandType.Text);
        }

        /// <summary>     
        /// 执行一个查询语句，返回查询结果的第一行第一列     
        /// </summary>     
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>     
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <returns></returns>     
        public Object ExecuteScalar(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            using (DbCommand command = CreateDbCommand(sql, parameters, commandType))
            {
                command.Connection.Open();
                object result = command.ExecuteScalar();
                command.Connection.Close();
                return result;
            }
        }

        public DbParameter CreateDbParameter(string name, object value)
        {
            return CreateDbParameter(name, ParameterDirection.Input, value);
        }

        public DbParameter CreateDbParameter(string name, ParameterDirection parameterDirection, object value)
        {
            DbParameter parameter = providerFactory.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.Direction = parameterDirection;
            return parameter;
        }

        /// <summary>  
        /// 创建一个DbCommand对象  
        /// </summary>  
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>  
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <returns></returns>  
        private DbCommand CreateDbCommand(string sql, IList<DbParameter> parameters, CommandType commandType)
        {
            DbConnection connection = providerFactory.CreateConnection();
            DbCommand command = providerFactory.CreateCommand();
            connection.ConnectionString = _connectionString;
            command.CommandText = sql;
            command.CommandType = commandType;
            command.Connection = connection;
            if (!(parameters == null || parameters.Count == 0))
            {
                foreach (DbParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }
    }
}