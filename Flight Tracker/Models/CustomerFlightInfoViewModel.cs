using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Models
{
    public class CustomerFlightInfoViewModel
    {
        public Customer Customer { get; set; }
        public FlightInfo FlightInfo { get; set; }
        public List<FlightInfo> Flights { get; set; }
    }
}
