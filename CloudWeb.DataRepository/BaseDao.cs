using System.Linq;
using System.Data;

namespace CloudWeb.DataRepository
{
    public class BaseDao<T> where T : class, new()
    {
        DapperHelper dapper = new DapperHelper();
        IDbConnection conn = null;

        public string sql = "";
        public BaseDao()
        {
            //调用数据库
            conn = dapper.SqlConnection();
        }

        public bool Add(string sql, dynamic t = null)
        {
            using (conn)
            {
                return dapper.Execute(sql, t);
            }
        }

        public bool Delete(string sql, dynamic t = null)
        {
            using (conn)
            {
                return dapper.Execute(sql, t);
            }
        }

        public T Find(string sql, dynamic t = null)
        {
            using (conn)
            {
                return dapper.Execute(sql, t);
            }
        }

        public IQueryable<T> GetAll(string sql, dynamic t = null)
        {
            using (conn)
            {
                return dapper.Execute(sql, t);
            }
        }

        public bool Update(string sql, dynamic t = null)
        {
            using (conn)
            {
                return dapper.Execute(sql, t);
            }
        }
    }
}
