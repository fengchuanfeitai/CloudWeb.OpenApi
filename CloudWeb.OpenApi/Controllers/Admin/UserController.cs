using CloudWeb.Dto;
using CloudWeb.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudWeb.OpenApi.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _service;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public UserController(ILogger<UserController> logger, IUserService service)
        {
            _logger = logger;
            service = _service;
        }

        [HttpGet]


        public IEnumerable<UserDto> GetUsersAsync() => _service.GetAllAsync();


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostUserAsync([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _service.AddAsync(user);

            return CreatedAtRoute(nameof(GetUserByIdAsync), new { id = user.Id });
        }

        //get api/bookchaptersAsync/guid
        [HttpGet("{id}", Name = nameof(GetUserByIdAsync))]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            UserDto user = _service.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return new ObjectResult(user);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookChapters(int id, [FromBody] UserDto user)
        {
            if (id == null || user == null)
                return BadRequest();

            if (_service.FindAsync(id) == null)
                return NotFound();

            _service.UpdateAsync(user);
            return new NoContentResult();
        }

        //delete api/bookchapters/4
        [HttpDelete("{id}")]

        public async Task DeleteAsync(dynamic[] ids) => _service.RemoveAsync(ids);
    }
}
