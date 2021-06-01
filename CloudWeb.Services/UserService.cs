using System;
using System.Linq;
using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;
using CloudWeb.OpenApi.Core.Core.Jwt.UserClaim;
using CloudWeb.OpenApi.Core.Jwt;
using CloudWeb.Util;

namespace CloudWeb.Services
{
    public class UserService : BaseDao, IUserService
    {
        private readonly JwtService _jwtService;

        public UserService(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResponseResult<UserData> Login(UserParam dto)
        {
            //登录成功
            ResponseResult<UserData> result = LoginValidate(dto);

            if (result.code != (int)HttpStatusCode.OK)
            {
                return result;
            }
            //返回token,user信息
            result.data.Token = _jwtService.BuildToken(_jwtService.BuildClaims(result.data));
            return new ResponseResult<UserData>(result.data);
        }


        public ResponseResult<UserData> LoginValidate(UserParam dto)
        {
            ResponseResult<UserData> result = new ResponseResult<UserData>();
            //将登录用户查出来
            string sql = "select password from users where isdel=0 and UserName=@UserName;";

            var hashPassword = Find<string>(sql, new { UserName = dto.UserName });


            //用户不存在
            if (hashPassword is null)
            {
                return result.SetFailMessage("用户名或密码不正确！");
            }

            if (Crypto.VerifyHashedPassword(hashPassword, dto.PassWord))
            {
                UserData userInDB
          = Find<UserData>(@"select r.*,u.* from users u left join roles r on u.roleID = r.roleid  where u.isdel=0 and UserName=@account;", new { account = dto.UserName });
                //密码正确后才加载用户信息、角色信息
                //得到userdata

                return result.SetData(userInDB);
            }
            //密码错误
            else
            {
                return result.SetFailMessage("用户名或密码不正确！");

            }
        }

    }
}
