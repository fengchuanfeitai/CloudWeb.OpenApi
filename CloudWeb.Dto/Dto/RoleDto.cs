using CloudWeb.Dto.Common;

namespace CloudWeb.Dto
{
    public class RoleDto : BaseDto
    {
        public int Id { get; set; }

        public string RoleName { get; set; }
        public UserDto user { get; set; }
    }
}
