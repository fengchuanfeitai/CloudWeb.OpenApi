using System.Collections.Generic;

namespace CloudWeb.DataRepository
{
    public class BaseDao<T> where T : class, new()
    {
        DapperHelper dapper = new DapperHelper();

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Add(string sql, dynamic t = null)
        {
            return dapper.Execute(sql, t) > 0;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Delete(string sql, dynamic t = null)
        {
            return dapper.Execute(sql, t) > 0;
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Find(string sql, dynamic t = null)
        {
            return dapper.QueryFirstOrDefault<T>(sql, t);
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public int Count(string sql, dynamic t = null)
        {
            return dapper.QueryFirstOrDefault<int>(sql, t);
        }


        /// <summary>
        /// 查询所有集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll(string sql, dynamic t = null)
        {
            return dapper.Query<T>(sql, t);
        }

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Update(string sql, dynamic t = null)
        {
            return dapper.Execute(sql, t) > 0;
        }
    }
}
