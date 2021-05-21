using System.Collections.Generic;

namespace CloudWeb.DataRepository
{
    public class BaseDao<T> where T : class, new()
    {
        DapperHelper dapper = new DapperHelper();

        public BaseDao()
        {
        }

        public bool Add(string sql, dynamic t = null)
        {
            return dapper.Execute(sql, t);
        }

        public bool Delete(string sql, dynamic t = null)
        {

            return dapper.Execute(sql, t);

        }

        public T Find(string sql, dynamic t = null)
        {

            return dapper.QueryFirstOrDefault<T>(sql, t);

        }

        public IEnumerable<T> GetAll(string sql, dynamic t = null)
        {

            return dapper.Query<T>(sql, t);

        }

        public bool Update(string sql, dynamic t = null)
        {

            return dapper.Execute(sql, t);

        }
    }
}
