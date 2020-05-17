using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Models
{

    public class DataInfo
    {
        public Pagination pagination { get; set; }
        public Datum[] data { get; set; }
    }

    public class Pagination
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public int total { get; set; }
    }

    public class Datum
    {
        public string flight_date { get; set; }
        public string flight_status { get; set; }
        public Departure departure { get; set; }
        public Arrival arrival { get; set; }
        public Airline airline { get; set; }
        public Flight flight { get; set; }
        public Aircraft aircraft { get; set; }
        public Live live { get; set; }
    }

    public class Departure
    {
        public string airport { get; set; }
        public string timezone { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public string terminal { get; set; }
        public string gate { get; set; }
        public int? delay { get; set; }
        public DateTime? scheduled { get; set; }
        public DateTime? estimated { get; set; }
        public DateTime? actual { get; set; }
        public DateTime? estimated_runway { get; set; }
        public DateTime? actual_runway { get; set; }
    }

    public class Arrival
    {
        public string airport { get; set; }
        public string timezone { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public string terminal { get; set; }
        public string gate { get; set; }
        public string baggage { get; set; }
        public int? delay { get; set; }
        public DateTime? scheduled { get; set; }
        public DateTime? estimated { get; set; }
        public DateTime? actual { get; set; }
        public DateTime? estimated_runway { get; set; }
        public DateTime? actual_runway { get; set; }
    }

    public class Airline
    {
        public string name { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
    }

    public class Flight
    {
        public string number { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public CodeShared codeShared { get; set; }
    }

    public class CodeShared
    {
        public string airline_name { get; set; }
        public string airline_iata { get; set; }
        public string airline_icao { get; set; }
        public string flight_number { get; set; }
        public string flight_iata { get; set; }
        public string flight_icao { get; set; }
    }

    public class Aircraft
    {
        public string registration { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public string icao24 { get; set; }
    }

    public class Live
    {
        public DateTime? updated { get; set; }
        public float? latitude { get; set; }
        public float? longitude { get; set; }
        public float? altitude { get; set; }
        public float? direction { get; set; }
        public float? speed_horizontal { get; set; }
        public float? speed_vertical { get; set; }
        public bool is_ground { get; set; }
    }


}
