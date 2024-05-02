using Microsoft.EntityFrameworkCore;

namespace API_livechat.Models
{
    public class loginContext : DbContext
    {
        public loginContext(DbContextOptions<loginContext> options) : base(options) { }

        public DbSet<Userl> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Userl>()
                .HasKey(u => new { u.UserId });
        }
    }
}
