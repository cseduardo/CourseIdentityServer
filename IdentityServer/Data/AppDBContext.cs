using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    /// <summary>
    /// Identity contains all the user tables
    /// </summary>
    public class AppDBContext:IdentityDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options){}
    }
}
