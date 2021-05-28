using System;
using System.Collections.Generic;
using System.Text;

namespace CloudWeb.OpenApi.Core.Core.Jwt.UserClaim
{
    public class UserData
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string RoleName { get; set; }
        public string Role { get; set; }

        public string Token { get; set; }
    }
}
