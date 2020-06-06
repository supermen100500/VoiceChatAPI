using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoiceChatAPI.Application.Exceptions;
using VoiceChatAPI.Application.Interfaces;
using VoiceChatAPI.Application.Models.User;
using VoiceChatAPI.Domain.Interfaces;
using VoiceChatAPI.Domain.Models;
using VoiceChatAPI.Security;
using VoiceChatAPI.Security.Models;

namespace VoiceChatAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<AppUser> _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthOptions _authOptions;
        private readonly IMapper _mapper;

        public UserService(IUserRepository<AppUser> userRepository,
            UserManager<AppUser> userManager,
            AuthOptions authOptions,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _authOptions = authOptions;
            _mapper = mapper;
        }

        public IEnumerable<AppUserData> GetUsers()
        {
            return _mapper.Map<IEnumerable<AppUserData>>(_userRepository.ListAll());
        }

        public AppUserData GetCurrentUser(string userName)
        {
            return _mapper.Map<AppUserData>(_userRepository.FindByName(userName));
        }

        public async Task<JWTPair> RegisterAsync(UserRegisterData registerData)
        {
            var user = await _userManager.FindByEmailAsync(registerData.Email);

            if (user != null) throw new BadRequestException("Пользователь с таким Email уже зарегистрирован!");

            var appUser = _mapper.Map<AppUser>(registerData);

            var x = await _userManager.CreateAsync(appUser, registerData.Password);

            if (x.Succeeded)
            {
                return _authOptions.GenerateJWTToken(appUser);
            }

            throw new BadRequestException("Пароль пользователя не соотетствует требованиям безопасности!");
        }

        public async Task<JWTPair> LoginAsync(UserRegisterData registerData)
        {
            var appUser = await _userManager.FindByEmailAsync(registerData.Email);

            if (appUser != null && await _userManager.CheckPasswordAsync(appUser, registerData.Password))
                return _authOptions.GenerateJWTToken(appUser);
            else
                throw new BadRequestException("Пользователь с такой парой логин/пароль не найден!");
        }


    }
}
