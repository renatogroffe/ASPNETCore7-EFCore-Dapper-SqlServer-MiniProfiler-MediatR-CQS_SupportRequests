using Microsoft.EntityFrameworkCore;

namespace APISupport.Data
{
    public class SupportContext : DbContext
    {
        public DbSet<SupportRequest>? SupportRequests { get; set; }

        public SupportContext(DbContextOptions<SupportContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupportRequest>().HasKey(r => r.Id);
        }
    }
}