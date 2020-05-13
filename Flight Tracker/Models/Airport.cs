using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight_Tracker.Models
{
    public class Airport
    {
        public string code { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string utc { get; set; }
        public int rightnow { get; set; }
        public string rightnow_description { get; set; }
        public int user_reported { get; set; }
        public string precheck { get; set; }
        public Faa_Alerts faa_alerts { get; set; }
        public Estimated_Hourly_Times[] estimated_hourly_times { get; set; }
        public Precheck_Checkpoints precheck_checkpoints { get; set; }
    }

    public class Faa_Alerts
    {
        public Ground_Stops ground_stops { get; set; }
        public Ground_Delays ground_delays { get; set; }
        public General_Delays general_delays { get; set; }
    }

    public class Ground_Stops
    {
        public string reason { get; set; }
        public string end_time { get; set; }
    }

    public class Ground_Delays
    {
        public string reason { get; set; }
        public string average { get; set; }
    }

    public class General_Delays
    {
        public string reason { get; set; }
        public string trend { get; set; }
    }

    public class Precheck_Checkpoints
    {
        public TerminalMAIN TerminalMAIN { get; set; }
    }

    public class TerminalMAIN
    {
        public string ConcourseCCheckpoint { get; set; }
        public string ConcourseDCheckpoint { get; set; }
    }

    public class Estimated_Hourly_Times
    {
        public string timeslot { get; set; }
        public float waittime { get; set; }
    }

}

