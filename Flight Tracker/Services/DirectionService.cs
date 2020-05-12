using Flight_Tracker.Models;
using MySql.Data.MySqlClient.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flight_Tracker.Services
{
    public class DirectionService
    {
        public DirectionService()
        {

        }

        public string GetDirectionsURL(Customer customer)
        {
            return $"https://maps.googleapis.com/maps/api/directions/json?origin={customer.StreetAddress}&destination=Chicago&traffic_model=best_guess&departure_time=now&key={APIKeys.GoogleAPI}";
        }

        public async Task<Customer> GetDirections(Customer customer);
        {
            string apiurl = GetDirectionsURL(customer);
            using (HttpClient client = new HttpClient())
            {
                Client.BaseAddress = new Uri(apiurl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue);
                HttpRequestMessage response = await client.GetAsync(apiurl);

    HttpresponseMessage respones
            }
        }
    }
}
