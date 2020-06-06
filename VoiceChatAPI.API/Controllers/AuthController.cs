using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VoiceChatAPI.API.Models;
using VoiceChatAPI.Application.Interfaces;
using VoiceChatAPI.Application.Models.User;
using VoiceChatAPI.Security.Models;

namespace VoiceChatAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<JWTPair> Register([FromBody]UserRegisterData model)
        {
            if (ModelState.IsValid)
            {
                var userData = new UserRegisterData() { Email = model.Email, Password = model.Password };
                return await _userService.RegisterAsync(userData);
            }

            return null;
        }

        [HttpPost("login")]
        public async Task<JWTPair> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                return await _userService.LoginAsync(_mapper.Map<UserRegisterData>(model));
            }

            return null;
        }
    }
}
