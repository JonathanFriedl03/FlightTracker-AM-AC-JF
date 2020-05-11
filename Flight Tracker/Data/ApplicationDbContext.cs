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
       // public DbSet<FlightTracker> Flights { get; set; } NOT SURE IF WE GONNA USE THIS YET
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
<<<<<<< HEAD
            builder.Entity<IdentityRole>().HasData(
            //new IdentityRole
            //{
            //    Name = "Admin",
            //    NormalizedName = "ADMIN"
            //}
            new IdentityRole
=======
            builder.Entity<IdentityRole>().HasData(new IdentityRole
>>>>>>> 53c60733823853d328db021fc87edc83f16be481
            {
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            }
<<<<<<< HEAD
            //new IdentityRole
            //{
            //    Name = "Employee",
            //    NormalizedName = "EMPLOYEE"
            //}
            );
        }
       // public DbSet<Flight_Tracker.Models.Customer> Customer { get; set; }
=======
            );
        }
        public DbSet<Customer> Customers { get; set; }
>>>>>>> 53c60733823853d328db021fc87edc83f16be481
    }
}