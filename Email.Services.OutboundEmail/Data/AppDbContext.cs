using Email.Services.OutboundEmail.Models;
using Microsoft.EntityFrameworkCore;

namespace Email.Services.OutboundEmail.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }
        public DbSet<MailRequest> MailRequests { get; set; }
        public DbSet<EmailConfig> EmailConfigs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MailRequest>()
           .Ignore(m => m.Attachments);
        }
    }
}
