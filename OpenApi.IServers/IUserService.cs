using System.Collections.Generic;
using System.Threading.Tasks;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;
using CloudWeb.OpenApi.Core.Core.Jwt.UserClaim;

namespace CloudWeb.IServices
{
    public interface IUserService : IBaseService
    {
        ResponseResult<UserData> Login(string name, string password);
    }

}
