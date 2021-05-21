using System.Collections.Generic;
using System.Threading.Tasks;
using CloudWeb.Dto;
using CloudWeb.Dto.Param;
using CloudWeb.IServices;

namespace CloudWeb.IServices
{
    public interface IUserService : IBaseService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<UserDto> FindAsync(int id);

        Task<bool> AddAsync(UserDto user);

        Task<bool> UpdateAsync(UserDto user);

        Task<bool> RemoveAsync(int id);

        Task<bool> IsExistsAsync(int id);

        Task<UserDto> Login(string name, string password);

    }

}
