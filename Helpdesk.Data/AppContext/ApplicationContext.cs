using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Helpdesk.Model.Entities;

namespace Helpdesk.Data.AppContext
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationUserRoleMapping> ApplicationUserRoleMapping { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<CompanyAgentMapping> CompanyAgentMapping { get; set; }
        public DbSet<CompanyUser> CompanyUser { get; set; }
        public DbSet<CompanyUserRoleMapping> CompanyUserRoleMapping { get; set; }
        public DbSet<Demand> Demands { get; set; }
        public DbSet<OrderOfUrgency> OrderOfUrgency { get; set; }

    }
}
