using AutoMapper;
using VoiceChatAPI.Application.Models.User;
using VoiceChatAPI.Domain.Models;

namespace VoiceChatAPI.Application.Mappings.User
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<AppUser, AppUserData>().ReverseMap();
            CreateMap<UserRegisterData, AppUser>()
                .ForMember(u => u.UserName, map => map.MapFrom(u => u.Email));
        }
    }
}
