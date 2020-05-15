using Flight_Tracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient.Memcached;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Flight_Tracker.Contracts;

namespace Flight_Tracker.Services
{
    public class DirectionService //: IDirectionsRequest not sure y this isnt working
    {
        public DirectionService()
        {

        }

        public async Task<TravelInfo> GetDirections(Customer customer)
        {
            string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={customer.StreetAddress}&{customer.ZipCode}&destination={customer.Datum.departure.airport}&traffic_model=best_guess&departure_time=now&key={APIKeys.GoogleAPI}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            TravelInfo travelInfo = null;

                if (response.IsSuccessStatusCode)
                {
                    string data =  await response.Content.ReadAsStringAsync();
                    travelInfo = JsonConvert.DeserializeObject<TravelInfo>(data);
                    return travelInfo;
                    

                    //customer.Latitude = (double)location["lat"];
                    //customer.Longitude = (double)location["lng"];
                   // customer.Latitude = (double)location[];
                    
                }
                return null;
            
        }
    }
}
