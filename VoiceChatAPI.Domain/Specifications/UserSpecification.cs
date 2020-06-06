using VoiceChatAPI.Domain.Models;

namespace VoiceChatAPI.Domain.Specifications
{
    public class UserSpecification<T> : BaseSpecification<T> where T : AppUser
    {
        public UserSpecification(string userName) : base(u => u.UserName.ToLower() == userName.ToLower())
        {
        }
    }
}
