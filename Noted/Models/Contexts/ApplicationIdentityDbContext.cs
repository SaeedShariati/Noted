using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Noted.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noted.Models
{
    public class ApplicationIdentityDbContext:IdentityDbContext<AppUser,IdentityRole,string>
    {
        public ApplicationIdentityDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Note> Notes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>().Property(p => p.Created).HasDefaultValueSql("getdate()");
            base.OnModelCreating(builder);
            /*uilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
            });*/
            builder.Entity<AppUser>().Property(p => p.UserName).IsRequired();
            builder.Entity<AppUser>().Property(p => p.Id).HasDefaultValueSql("newid()");
            builder.Entity<Note>().Property(p => p.Created).HasDefaultValueSql("getdate()");
        }
    }
}
