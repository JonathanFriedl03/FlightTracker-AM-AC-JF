using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Models
{
    public class FlightInfo
    {
        [Key]
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string Airport { get; set; }
        public string ArrivalAirport { get; set; }
        public string Airline { get; set; }
        public string FlightStatus { get; set; }
        public string Gate { get; set; }
        public int? Delay { get; set; }
        public string AirportCode { get; set; }
        public string ArrivalAirportCode { get; set; }
        public DateTime? EstimatedDeparture { get; set; }
        public DateTime? ActualDeparture { get; set; }
        public DateTime? EstimatedArrival { get; set; }
        public DateTime? ActualArrival { get; set; }
        public TimeSpan? SelectedArrivalTime { get; set; }
        public double? TSAWaitTimeOnArrival { get; set; }
        public long EpochTime { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
