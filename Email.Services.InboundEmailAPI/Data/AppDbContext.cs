using Email.Services.InboundEmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Email.Services.InboundEmailAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<InboundEmail> InboundEmails { get; set; }
        public DbSet<EmailAttachments> EmailAttachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
