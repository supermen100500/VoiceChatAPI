using System.Collections.Generic;
using System.Threading.Tasks;
using VoiceChatAPI.Application.Models.User;
using VoiceChatAPI.Security.Models;

namespace VoiceChatAPI.Application.Interfaces
{
    public interface IUserService
    {
        IEnumerable<AppUserData> GetUsers();
        AppUserData GetCurrentUser(string userName);
        Task<JWTPair> RegisterAsync(UserRegisterData registerData);
        Task<JWTPair> LoginAsync(UserRegisterData registerData);
    }
}
