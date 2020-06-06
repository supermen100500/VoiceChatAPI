using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VoiceChatAPI.Domain.Models;

namespace VoiceChatAPI.Domain
{
    public class VCDbContext : IdentityDbContext<AppUser>
    {
        public VCDbContext(DbContextOptions<VCDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
