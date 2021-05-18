using CloudWeb.DataRepository;
using CloudWeb.IServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CloudWeb.Services
{
    public class BaseService<T> where T : class, new()
    {
        DapperHelper dapper = new DapperHelper();
        IDbConnection conn = null;

        public string sql = "";
        public BaseService()
        {
            //调用数据库
            conn = dapper.SqlConnection();
        }

        public IEnumerable<T> GetAllAsync()
        {
            using (conn)
            {
                return dapper.Query<T>(sql);
            }
        }

        public T FindAsync(int id)
        {
            return dapper.QueryFirstOrDefault<T>(sql);
        }

        public bool AddAsync(T user)
        {
            return dapper.Execute(sql) > 0;
        }

        public bool UpdateAsync(T user)
        {
            return dapper.Execute(sql) > 0;
        }

        public bool RemoveAsync(dynamic[] ids)
        {
            return dapper.Execute(sql) > 0;
        }

        public bool IsExistsAsync(int id)
        {
            return dapper.Execute(sql) > 0;
        }
    }
}
