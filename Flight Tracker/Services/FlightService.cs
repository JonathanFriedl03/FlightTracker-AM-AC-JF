using Flight_Tracker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flight_Tracker.Services
{
    public class FlightService
    {
        public FlightService()
        {

        }
        public async Task<DataInfo> GetArrivalInfo(Customer customer)
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"http://api.aviationstack.com/v1/flights?access_key={APIKeys.FlightApiKey}&flight_iata={customer.FlightNumber}");
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                JObject jObject = JObject.Parse(json);
                //DateTime flightDate = (DateTime)jObject[]
                return JsonConvert.DeserializeObject<DataInfo>(json);
            }
            return null;
        }
    }
}
