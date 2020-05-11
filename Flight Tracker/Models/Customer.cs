using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string FlightNumber { get; set; }
        public string Airport { get; set; }
        public string FlightStatus { get; set; }
        public string Gate { get; set; }
        public int? Delay { get; set; }
        public DateTime? EstimatedDeparture { get; set; }
        public DateTime? ActualDeparture { get; set; }
        public DateTime? EstimatedArrival { get; set; }
        public DateTime? ActualArrival { get; set; }
        public List<Contact> Contacts { get; set; }
        public string UserName { get; internal set; }
        public string Email { get; internal set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
