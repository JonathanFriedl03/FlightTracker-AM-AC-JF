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

namespace Flight_Tracker.Services
{
    public class DirectionService
    {
        public DirectionService()
        {

        }

        //public string GetDirectionsURL(Customer customer)
        //{

        //}

        //public async Task<Customer> GetDirections(Customer customer)
        //{
        //    string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={customer.StreetAddress}&destination=Chicago&traffic_model=best_guess&departure_time=now&key={APIKeys.GoogleAPI}";
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(url);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage response = await client.GetAsync(url);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string data = await response.Content.ReadAsStringAsync();
        //            JObject jsonResults = JsonConvert.DeserializeObject<JObject>(data);
        //            // JToken results = jsonResults[];
        //        }
        //        return customer;
        //    }
        //}
    }
}
