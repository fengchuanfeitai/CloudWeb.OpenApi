using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;

namespace CloudWeb.Services
{
    public class UserService : BaseDao<UserDto>, IUserService
    {
        public bool AddAsync(UserDto user)
        {
            string sql = "insert into(1 )values(@1)";
            return Add(sql, user);
        }

        public UserDto FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserDto> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public bool IsExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAsync(dynamic[] ids)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAsync(UserDto user)
        {
            throw new NotImplementedException();
        }
    }
}
