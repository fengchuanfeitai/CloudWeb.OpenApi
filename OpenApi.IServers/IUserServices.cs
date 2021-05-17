using System.Collections.Generic;
using System.Threading.Tasks;
using CloudWeb.Dto;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;

namespace CloudWeb.IServers
{
    public interface IUserServices : IServiceTag
    {
        Task<IEnumerable<UserDto>> GetList();

        Task<UserDto> GetEntity(UserParam param);

        Task<bool> Add(UserDto user);

        Task<bool> Edit(UserDto user);

        Task<bool> Delete(string id);

        Task<bool> IsExists(string id);

        Task<UserDto> Login(string name, string password);

    }

}
