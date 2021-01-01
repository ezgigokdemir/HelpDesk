using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Mvc.Areas.Identity.Data;

namespace Web.Mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomIdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<CustomIdentityUser>().ToTable("AspNetUsers");
            //builder.Entity<IdentityRole>().ToTable("IdentityRole");

            //builder.Entity<IdentityUserRole<string>>().ToTable("IdentityUserRole");
            //builder.Entity<IdentityUserClaim<string>>().ToTable("IdentityUserClaim");
            //builder.Entity<IdentityUserLogin<string>>().ToTable("IdentityUserLogin");

            //builder.Entity<IdentityRoleClaim<string>>().ToTable("IdentityRoleClaim");
            //builder.Entity<IdentityUserToken<string>>().ToTable("IdentityUserToken");

            builder.Entity<CustomIdentityUser>().Property(p => p.AccountId);
            builder.Entity<CustomIdentityUser>().Property(p => p.FirstName).HasMaxLength(64);
            builder.Entity<CustomIdentityUser>().Property(p => p.FirstName).HasMaxLength(64);
        }
    }
}
