using System;
using System.Linq;
using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.IServices;

namespace CloudWeb.Services
{
    public class UserService : BaseDao<UserDto>, IUserService
    {
    }
}
