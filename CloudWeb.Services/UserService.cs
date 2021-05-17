using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudWeb.Dto;
using CloudWeb.Dto.Param;
using CloudWeb.IServers;

namespace CloudWeb.Services
{
    public class UserService : IUserService
    {
        public Task<bool> AddAsync(UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> FindAsync(UserParam param)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> FindAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Login(string name, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(UserDto user)
        {
            throw new NotImplementedException();
        }
    }
}
