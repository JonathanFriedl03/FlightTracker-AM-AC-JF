using Flight_Tracker.Contracts;
using Flight_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Data
{
    public class FlightRepository : RepositoryBase<FlightInfo>, IFlightRepository
    {
        public FlightRepository(ApplicationDbContext applicationDbContext) 
            : base(applicationDbContext) 
        { 
        }
        public List<FlightInfo> GetFlights(int customerId) =>
            FindByCondition(f => f.CustomerId.Equals(customerId)).ToList();
        public FlightInfo GetFlight(int flightId) =>
            FindByCondition(f => f.Id.Equals(flightId)).SingleOrDefault();
        public void CreateFlight(FlightInfo flight) => Create(flight);
        public void DeleteFlight(int flightId)
        {
            var flightToDelete = FindByCondition(c => c.Id.Equals(flightId)).SingleOrDefault();
            Delete(flightToDelete);
        }
        public void EditFlight(FlightInfo flight)
        {
            Update(flight);
        }
    }
}
