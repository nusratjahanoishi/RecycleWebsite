using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextUses.Models;


namespace NextUses.Data
{
    public class NextUsesDB : IdentityDbContext<Users>
    {
        public NextUsesDB(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Users> users { get; set; }
    }
}
