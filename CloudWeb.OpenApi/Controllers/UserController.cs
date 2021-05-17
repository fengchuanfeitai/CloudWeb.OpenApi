using CloudWeb.Dto;
using CloudWeb.Dto.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace CloudWeb.OpenApi.Controllers
{
    public class UserController : ApiController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService service;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            this.service = service;
        }

        // GET: User
        /// <summary>
        /// sadsd
        /// </summary>
        /// <returns></returns>
        [JwtAuthentication]

        public ResultDto<IEnumerable<UserDto>> GetList()
        {
            ResultDto<IEnumerable<UserDto>> result = new ResultDto<IEnumerable<UserDto>>();

            try
            {
                result.Data = service.GetList();
                result.Meta.Status = 200;
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.Meta.Status = 403;
                result.Meta.Msg = ex.Message;
            }

            return result;

        }
    }
}
