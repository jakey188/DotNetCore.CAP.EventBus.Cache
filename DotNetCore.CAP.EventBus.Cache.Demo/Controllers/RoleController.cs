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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("role/add")]
        public async Task<IActionResult> Add([FromBody] RoleEntity request)
        {
            var role = new RoleEntity
            {
                Id = Guid.NewGuid().ToString(),
                AppId = request.AppId,
                Name = request.Name
            };

            await _roleService.Add(role);

            return Ok(role);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("role/info")]
        public async Task<IActionResult> GetRoleAsync(string roleId)
        {
            var role = await _roleService.GetRoleAsync(roleId);
            return Ok(role);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        [HttpGet("role/GetRoleListByAppId")]
        public async Task<IActionResult> GetRoleListByAppId(string appId)
        {
            var roles = await _roleService.GetRoleListByAppIdAsync(appId);
            return Ok(roles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("role/delete")]
        public async Task<IActionResult> Delete(string roleId)
        {
            await _roleService.Delete(roleId);
            return Ok("ok");
        }
    }
}
