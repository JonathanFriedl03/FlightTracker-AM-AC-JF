using Flight_Tracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flight_Tracker.Services
{
    public class TSAWaitTimesService : ITSAWaitTimesService
    {
        public async Task<Airport> GetWaitTimes(string airportCode)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://www.tsawaittimes.com/api/airport/{APIKeys.TSAWaitTimesAPIKey}/{airportCode}/json");
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Airport>(json);
            }
            return null;
        }
    }
}
