using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudWeb.IServices
{
    public interface IBaseService<T> where T : class, new()
    {
        IQueryable<T> GetAllAsync();

        T FindAsync(int id);

        bool AddAsync(T user);

        bool UpdateAsync(T user);

        bool RemoveAsync(dynamic[] ids);

        bool IsExistsAsync(int id);

    }
}
