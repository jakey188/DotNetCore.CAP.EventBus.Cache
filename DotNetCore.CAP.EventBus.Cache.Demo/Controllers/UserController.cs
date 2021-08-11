using DotNetCore.CAP.Cap.EventBus.Cache.Data.Entites;
using DotNetCore.CAP.Cap.EventBus.Cache.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Web.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/info")]
        public async Task<IActionResult> GetUserInfoAsync(string userId)
        {
            var user = await _userService.GetUserAsync(userId);
            return Ok(user);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [HttpPost("user/add")]
        public async Task<IActionResult> Add([FromBody] UserEntity request)
        {
            var entity = new UserEntity
            {
                Id = Guid.NewGuid().ToString(),
                AppId = request.AppId,
                NickName = request.NickName,
                PhoneNumber = request.PhoneNumber
            };
            await _userService.Add(entity);
            return Ok(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("user/delete")]
        public async Task<IActionResult> Delete(string userId)
        {
            await _userService.Delete(userId);
            return Ok("ok");
        }
    }
}
