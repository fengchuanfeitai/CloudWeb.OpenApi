using Dapper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloudWeb.DataRepository
{
    /// <summary>
    /// dapper封装
    /// </summary>
    public class DapperHelper : IDisposable
    {
        //数据库连接字符串
        private readonly string ConnectionStr = "SqlConnectionStr";
        //初始化日志
        private ILog _log = LogManager.GetLogger(typeof(DapperHelper));
        //数据库访问对象
        public IDbConnection Connection { get; protected set; }

        public string ConnectionString = "";

        public DapperHelper()
        {
            //创建连接对象
            ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStr].ConnectionString;
        }

        /// <summary>
        /// sqlserver连接方法
        /// </summary>
        /// <returns></returns>
        public SqlConnection SqlConnection()
        {
            try
            {
                var connection = new SqlConnection(ConnectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                //打印错误连接日志
                _log.Error(string.Concat("SqlConnectionError: ", ex));
                throw;
            }
        }

        /// <summary>
        /// 事务
        /// </summary>
        private IDbTransaction _transaction;

        /// <summary>
        /// 事务
        /// </summary>
        public IDbTransaction Transaction
        {
            get { return _transaction; }
            set
            {
                _transaction = value;
                if (_transaction != null)
                    Connection = _transaction.Connection;
            }
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            _transaction = Connection.BeginTransaction();
            return _transaction;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="isolationLevel">事务等级</param>
        /// <returns></returns>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            _transaction = Connection.BeginTransaction(isolationLevel);
            return _transaction;
        }

        /// <summary>
        /// 查询集合方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="transaction">事务：默认为null</param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout">超时时间：默认null</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return SqlMapper.Query(Connection, sql, param, transaction, buffered, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("QueryError: ", ex));
                throw;
            }
            finally
            {
                if (buffered)
                    CloseConnection();
            }
        }

        /// <summary>
        /// 异步查询集合方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="transaction">事务：默认为null</param>
        /// <param name="commandTimeout">超时时间：默认null</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return await SqlMapper.QueryFirstOrDefaultAsync<T>(Connection, sql, param, transaction, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("QueryError: ", ex));
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// 异步查询集合方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="transaction">事务：默认为null</param>
        /// <param name="commandTimeout">超时时间：默认null</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return SqlMapper.QueryFirstOrDefault<T>(Connection, sql, param, transaction, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("QueryError: ", ex));
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }


        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered">是否缓存</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return SqlMapper.Query<T>(Connection, sql, param, transaction, buffered, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("QueryError: ", ex));
                throw;
            }
            finally
            {
                if (buffered)
                    CloseConnection();
            }
        }

        /// <summary>
        /// 异步查询集合
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return await SqlMapper.QueryAsync<T>(Connection, sql, param, transaction, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("QueryError: ", ex));
                throw;
            }
            finally
            {
                if (buffered)
                    CloseConnection();
            }
        }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="map">委托</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered">是否缓存</param>
        /// <param name="splitOn">连表id</param>
        /// <param name="commandTimeout">超时</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return SqlMapper.Query<TFirst, TSecond, TReturn>(Connection, sql, map, param, transaction, buffered, splitOn,
                                                                 commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("QueryError: ", ex));
                throw;
            }
            finally
            {
                if (buffered)
                    CloseConnection();
            }
        }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="map">委托</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered">是否缓存</param>
        /// <param name="splitOn">连表id</param>
        /// <param name="commandTimeout">超时</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Connection, sql, map, param, transaction, buffered, splitOn,
                                                                 commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("QueryError: ", ex));
                throw;
            }
            finally
            {
                if (buffered)
                    CloseConnection();
            }
        }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="map">委托</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered">缓存</param>
        /// <param name="splitOn">连表id</param>
        /// <param name="commandTimeout">超时</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TReturn>(Connection, sql, map, param, transaction, buffered, splitOn,
                                                                 commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("QueryError: ", ex));
                throw;
            }
            finally
            {
                if (buffered)
                    CloseConnection();
            }
        }

        /// <summary>
        /// 连表查询
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="map">委托</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered">缓存</param>
        /// <param name="splitOn">连表id</param>
        /// <param name="commandTimeout">超时</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return SqlMapper.Query<TFirst, TSecond, TThird, TReturn>(Connection, sql, map, param, transaction, buffered, splitOn,
                                                                 commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("QueryError: ", ex));
                throw;
            }
            finally
            {
                if (buffered)
                    CloseConnection();
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public int Execute(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return SqlMapper.Execute(Connection, sql, param, transaction, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("ExecuteError: ", ex));
                throw;
            }
            finally
            {
                if (buffered)
                    CloseConnection();
            }
        }


        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return await SqlMapper.ExecuteAsync(Connection, sql, param, transaction, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("ExecuteError: ", ex));
                throw;
            }
            finally
            {
                if (buffered)
                    CloseConnection();
            }
        }

        /// <summary>
        /// 多结果查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <param name="commandType">查询类型</param>
        /// <returns></returns>
        public SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (transaction == null)
                transaction = _transaction;

            OpenConnection();
            try
            {
                PrintSqlAndParam(sql, false, param);
                return SqlMapper.QueryMultiple(Connection, sql, param, transaction, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("QueryError: ", ex));
                throw;
            }
        }

        /// <summary>
        /// 通过事务执行多个sql
        /// </summary>
        /// <param name="sqls">sql语句集合</param>
        /// <returns></returns>
        public bool ExecuteWithTransaction(List<string> sqls)
        {
            Connection.Open();

            IDbTransaction trans = Connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                foreach (string sql in sqls)
                {
                    try
                    {
                        PrintSqlAndParam(sql, true);
                        SqlMapper.Execute(Connection, sql);
                    }
                    catch (Exception ex)
                    {
                        _log.Error(string.Concat("ExecuteWithTransactionError: ", ex));
                        return false;
                    }
                }

                trans.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _log.Error(string.Concat("TransactionError: ", ex));
                return false;
            }
            finally
            {
                trans.Rollback();
                Connection.Close();
            }
        }

        /// <summary>
        /// 执行事务带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public bool ExecuteWithTransaction(string sql, dynamic param = null)
        {
            return ExecuteWithTransaction((trans) =>
            {
                return Execute(sql, param, trans, false);
            });
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="func">委托方法</param>
        /// <returns></returns>
        private bool ExecuteWithTransaction(Func<IDbTransaction, int> func)
        {
            Connection.Open();

            IDbTransaction trans = Connection.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                var r = func(trans);

                trans.Commit();
                return r > 0;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                _log.Error(string.Concat("TransactionError: ", ex));

                return false;
            }
            finally
            {
                Connection.Close();
            }

        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        private void OpenConnection()
        {
            if (Connection != null && Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        private void CloseConnection()
        {
            if (Connection != null)
                Connection.Close();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();

            if (Connection != null)
                Connection.Dispose();

            _transaction = null;
            Connection = null;
        }

        /// <summary>
        /// 打印参数，记录日志
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="withTransaction">事务</param>
        /// <param name="param">参数</param>
        private void PrintSqlAndParam(string sql, bool withTransaction, dynamic param = null)
        {
            string prefix = withTransaction ? "ExecuteWithTransactionSql: " : "ExecuteSql: ";
            _log.Debug(string.Concat(prefix, sql));

            if (param != null)
            {
                string paramString = "";
                foreach (PropertyInfo property in param.GetType().GetProperties())
                {
                    paramString += $"Param[{property.Name}] = {property.GetValue(param)} ; ";
                }
                _log.Debug(paramString);
            }
        }


    }
}
