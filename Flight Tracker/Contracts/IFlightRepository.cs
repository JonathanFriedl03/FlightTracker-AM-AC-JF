using Flight_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Contracts
{
    public interface IFlightRepository
    {
        FlightInfo GetFlight(int flightId);
        List<FlightInfo> GetFlights(int customerId);
        void CreateFlight(FlightInfo flight);
        void DeleteFlight(int flightId);
        void EditFlight(FlightInfo flight);
    }
}
