using CloudWeb.Dto.Common;

namespace CloudWeb.Dto
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserDto : BaseDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string FaceImg { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 权限验证
        /// </summary>
        public string Token { get; set; }

        public RoleDto roleDto { get; set; }
    }
}
