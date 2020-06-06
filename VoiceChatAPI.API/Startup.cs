using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VoiceChatAPI.Application.Interfaces;
using VoiceChatAPI.Application.Models;
using VoiceChatAPI.Application.Services;
using VoiceChatAPI.Domain;
using VoiceChatAPI.Domain.Interfaces;
using VoiceChatAPI.Domain.Models;
using VoiceChatAPI.Domain.Repositories;
using VoiceChatAPI.Security;

namespace VoiceChatAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AssemblyLoader.LoadVoiceChatAssemblies(typeof(Program));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<VCDbContext>(options => options.UseNpgsql(connection));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<VCDbContext>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository<AppUser>, UserRepository<AppUser>>();
            services.AddScoped<AuthOptions>();

            services.AddAuthentication(SetAuthenticationOptions)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        LifetimeValidator = AuthOptions.CustomLifetimeValidator,

                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddControllers();

            services.AddAutoMapper(AssemblyLoader.Assemblies);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void SetAuthenticationOptions(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
