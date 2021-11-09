using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace packagesentinel.Models
{
    public class APIContext : IdentityDbContext<ApplicationUser>
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {

        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
       public DbSet<Package> Package { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser() { Id = new Guid().ToString(),DeviceId=1,UserName="admin",Email="admin",NormalizedEmail="admin",NormalizedUserName="admin",SecurityStamp=new Guid().ToString(),PasswordHash= hasher.HashPassword(null, "admin"), });
            builder.Entity<Device>().HasData(new Device() { Id = 1, Number = "123", Note = "created by seeding", Version = "v1.9", CreatedAt = DateTime.Now });
            builder.Entity<Setting>().HasData(new Setting(){DeviceId=1,Id=1});
            builder.Entity<Package>().HasData(new Package() {Id=1,DeviceId=1,PlacedAt= DateTime.Now});

        }
    }
}


