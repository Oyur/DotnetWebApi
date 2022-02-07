using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotnetWebApi.Authentication
{
    public class AuthenticateDBContext : IdentityDbContext<ApplicationUser>
    {
        public AuthenticateDBContext(DbContextOptions<AuthenticateDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}