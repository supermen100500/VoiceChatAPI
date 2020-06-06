using AutoMapper;
using VoiceChatAPI.API.Models;
using VoiceChatAPI.Application.Models.User;
using VoiceChatAPI.Domain.Models;

namespace VoiceChatAPI.API.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<UserLoginDto, UserRegisterData>().ReverseMap();
            CreateMap<UserRegisterData, AppUser>()
                .ForMember(u => u.UserName, map => map.MapFrom(u => u.Email));
            CreateMap<AppUserData, AppUserDto>().ReverseMap();
        }
    }
}
