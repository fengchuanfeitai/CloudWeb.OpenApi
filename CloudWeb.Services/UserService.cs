using System;
using System.Linq;
using CloudWeb.DataRepository;
using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using CloudWeb.IServices;
using CloudWeb.OpenApi.Core.Core.Jwt.UserClaim;
using CloudWeb.OpenApi.Core.Jwt;
using CloudWeb.Util;

namespace CloudWeb.Services
{
    public class UserService : BaseDao<UserDto>, IUserService
    {
        private readonly JwtService _jwtService;

        public UserService(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public ResponseResult<UserData> Login(string name, string password)
        {
            string sql = "";
            var result = LoginValidate(name, password);
            if (result.code==0)
            {
                result.data.Token = _jwtService.BuildToken(_jwtService.BuildClaims(result.data));
                return new ResponseResult<UserData>(result.data);
            }
            else
            {
                return new ResponseResult<UserData>(result.msg);
            }
        }

        public ResponseResult<UserData> Login(UserDto dto)
        {
            throw new NotImplementedException();
        }

        public ResponseResult<UserData> LoginValidate(string name, string password)
        {
            ResponseResult<UserData> result = new ResponseResult<UserData>();
            //将登录用户查出来
            string sql = "select * from users where account=@account and passsword=@password;";
            var loginUserInDB = Find(sql, new { name = name, password = password });

            //用户不存在
            if (loginUserInDB is null)
            {
                return result.SetFailMessage("用户不存在");
            }

            return result.SetFailMessage("用户不存在");
            ////密码正确
            //if (Crypto.VerifyHashedPassword(loginUserInDB.PassWord, password))
            //{
            //    //      DapperHelper cnn = new DapperHelper();
            //    //      var userInDB
            //    //= cnn.Query<UserData, RoleDto, UserData>(@"select * from Posts p left join Authors a on a.ID = p.AuthorID", (user, author) => { user.Id = author; return user; }, splitOn: "ID").ToList();
            //    //      //密码正确后才加载用户信息、角色信息
            //    //      //得到userdata
            //    //      return result.SetData(userInDB.First());
            //    return result.SetData("");
            //}
            ////密码错误
            //else
            //{
            //    return result.SetFailMessage("用户名或密码错误！");

            //}
        }

        ResponseResult<UserData> IUserService.Login(string name, string password)
        {
            throw new NotImplementedException();
        }
    }
}
