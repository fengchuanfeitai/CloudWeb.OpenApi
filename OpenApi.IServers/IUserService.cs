using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using CloudWeb.OpenApi.Core.Core.Jwt.UserClaim;

namespace CloudWeb.IServices
{
    public interface IUserService : IBaseService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ResponseResult<UserData> Login(UserParam user);
    }

}
