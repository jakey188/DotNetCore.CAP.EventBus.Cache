using DotNetCore.CAP.Cap.EventBus.Cache.Data.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Services
{
    public interface IRoleService
    {
        Task Add(RoleEntity entity);

        Task Delete(string roleId);

        Task<RoleEntity> GetRoleAsync(string roleId);

        Task<List<RoleEntity>> GetRoleListByAppIdAsync(string appId);
    }
}