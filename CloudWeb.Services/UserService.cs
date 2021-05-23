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
            var result = LoginValidate(dto);
            //返回token,user信息
            result.data.Token = _jwtService.BuildToken(_jwtService.BuildClaims(result.data));
            return new ResponseResult<UserData>(result.data);
        }


        public ResponseResult<UserData> LoginValidate(UserParam dto)
        {
            ResponseResult<UserData> result = new ResponseResult<UserData>();
            //将登录用户查出来
            string sql = "select password from users where isdel=0 account=@account and passsword=@password;";
            var hashPassword = Find<string>(sql, dto);

            //用户不存在
            if (hashPassword is null)
            {
                return result.SetFailMessage("用户名或密码不正确");
            }

            if (Crypto.VerifyHashedPassword(hashPassword, dto.PassWord))
            {
                DapperHelper cnn = new DapperHelper();
                var userInDB
          = cnn.Query<UserData>(@"select p.*,a.* from users p left join roles a on a.ID = p.AuthorID  where isdel=0 account=@account and passsword=@password;", dto );
                //密码正确后才加载用户信息、角色信息
                //得到userdata

                return result.SetData(userInDB as UserData);
            }
            //密码错误
            else
            {
                return result.SetFailMessage("用户名或密码错误！");

            }
        }

    }
}
