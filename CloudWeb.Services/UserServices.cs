using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudWeb.Dto;
using CloudWeb.Dto.Param;
using CloudWeb.IServers;

namespace CloudWeb.Services
{
    class UserServices : IUserServices
    {
        public Task<bool> Add(UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit(UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetEntity(UserParam param)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExists(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Login(string name, string password)
        {
            throw new NotImplementedException();
        }
    }
}
