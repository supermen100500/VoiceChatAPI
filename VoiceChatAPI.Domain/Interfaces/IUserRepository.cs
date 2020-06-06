using System.Threading.Tasks;
using VoiceChatAPI.Domain.Models;

namespace VoiceChatAPI.Domain.Interfaces
{
    public interface IUserRepository<T> : IRepository<T> where T : AppUser
    {
        Task RemoveUser(T user);
        Task<T> FindByName(string userName);
        Task<T> FindByEmail(string email);
        Task<bool> CheckPassword(T user, string password);
        Task<T> GetForReference(string username);
    }
}
