using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Flight_Tracker.Models;

namespace Flight_Tracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
       // public DbSet<FlightTracker> Flights { get; set; } NOT SURE IF WE GONNA USE THIS YET
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            }
            //new IdentityRole
            //{
            //    Name = "Customer",
            //    NormalizedName = "CUSTOMER"
            //},
            //new IdentityRole
            //{
            //    Name = "Employee",
            //    NormalizedName = "EMPLOYEE"
            //}
            );
        }
        public DbSet<Flight_Tracker.Models.Customer> Customer { get; set; }
    }
}