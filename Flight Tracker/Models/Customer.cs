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
        public string FlightDate { get; set; }

        public string FlightNumber { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        public int ZipCode { get; set; }
        public double? startLatitude { get; set; }
        public double? startLongitude { get; set; }
        public double? endLatitude { get; set; }
        public double? endLongitude { get; set; }
        public int? distance { get; set; }
        public int? duration { get; set; }
        [Display(Name = "Airport Name")]
        public string? Airport { get; set; }
        public string FlightStatus { get; set; }
        public string Gate { get; set; }
        public int? Delay { get; set; }
        public DateTime? EstimatedDeparture { get; set; }
        public DateTime? ActualDeparture { get; set; }
        public DateTime? EstimatedArrival { get; set; }
        public DateTime? ActualArrival { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public string UserName { get; internal set; }
        public string Email { get; internal set; }

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }


    }

}
