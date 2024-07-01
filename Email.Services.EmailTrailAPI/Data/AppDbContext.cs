﻿using Email.Services.EmailTrailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Email.Services.EmailTrailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<EmailTrail> EmailTrails { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

        }
    }
}
