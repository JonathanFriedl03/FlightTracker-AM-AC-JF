using Flight_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Services
{
    public interface ITSAWaitTimesService
    {
        Task<Airport> GetWaitTimes(string airportCode);
    }
}
