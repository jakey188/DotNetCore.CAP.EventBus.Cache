using DotNetCore.CAP.Cap.EventBus.Cache.Data.Entites;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Services
{
    public interface IUserService
    {
        Task Add(UserEntity entity);
        Task Delete(string userId);
        Task<UserEntity> GetUserAsync(string userId);
    }
}