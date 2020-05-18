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
        public int flightId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }                
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

        public IEnumerable<Contact> Contacts { get; set; }       

        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }

}
