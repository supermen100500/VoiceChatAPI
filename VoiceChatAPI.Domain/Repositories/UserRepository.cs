using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoiceChatAPI.Domain.Interfaces;
using VoiceChatAPI.Domain.Models;
using VoiceChatAPI.Domain.Specifications;

namespace VoiceChatAPI.Domain.Repositories
{
    public class UserRepository<T> : BaseRepository<T>, IUserRepository<T> where T : AppUser
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<AppUser> userManager,
            IMapper mapper,
            VCDbContext db) : base(db)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task RemoveUser(T user)
        {
            var appUser = await _userManager.FindByNameAsync(user.UserName);
            await _userManager.DeleteAsync(appUser);
            await Delete(user);
        }

        public async Task<bool> CheckPassword(T user, string password)
        {
            return await _userManager.CheckPasswordAsync(_mapper.Map<AppUser>(user), password);
        }

        public async Task<T> FindByEmail(string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            return appUser == null ? null : await GetSingleBySpec(new UserSpecification<T>(appUser.UserName));
        }

        public async Task<T> FindByName(string userName)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            return appUser == null ? null : await GetSingleBySpec(new UserSpecification<T>(appUser.UserName));
        }

        public async Task<T> GetForReference(string username)
        {
            var user = await FindByName(username);

            if (user == null) return null;

            return user;
        }

        private IQueryable<T> QueryUsers(IEnumerable<string> users)
        {
            var userNames = users.Distinct().Select(u => u.ToLower()).ToList();

            return Db.Set<T>().Where(s => userNames.Contains(s.UserName.ToLower()));
        }
    }
}
