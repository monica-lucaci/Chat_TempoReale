using Microsoft.EntityFrameworkCore;

namespace API_livechat.Models
{
    public class loginContext : DbContext
    {
        public loginContext(DbContextOptions<loginContext> options) : base(options) { }

        public DbSet<UserProfile> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                .HasKey(u => new { u.UserId });
        }
    }
}
