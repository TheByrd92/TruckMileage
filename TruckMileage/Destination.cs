using Newtonsoft.Json;
using System;
using System.Net;

namespace TruckMileage
{
    /// <summary>
    /// Destination class gives access to methods to explore Googles Directions API. It also implements the IDisposable.
    /// </summary>
    public class Destination : IDisposable
    {
        public string ogLocation = "3333 South Council Road, Oklahoma City, Oklahoma";
        private string API_KEY = "";

        /// <summary>
        /// <paramref name="API_KEY"/> is needed for any functions to work called to work.
        /// </summary>
        /// <param name="API_KEY">The API KEY gotten from google.</param>
        public Destination(string API_KEY)
        {
            this.API_KEY = API_KEY;
        }

        public void Dispose()
        {
            API_KEY = "";
            ogLocation = "";
        }

        /// <summary>
        /// This finds the mileage from the <seealso cref="ogLocation"/> and the location of <paramref name="fullStreetAddress"/> using Googles Direction API.<br></br>
        /// </summary>
        /// <param name="fullStreetAddress">Destination location</param>
        /// <returns>The mileage between the two destinations.</returns>
        public double FindMileage(string fullStreetAddress)
        {
            double toReturn = 0.0;
            try
            {
                string html = "https://maps.googleapis.com/maps/api/directions/json?origin=" + ogLocation + "&destination=" + fullStreetAddress + "&avoid=tolls|ferries&units=imperial&key=" + API_KEY;
                var client = new WebClient();
                var content = client.DownloadString(html);

                Rootobject pt = JsonConvert.DeserializeObject<Rootobject>(content);

                string dist = pt.routes[0].legs[0].distance.text;
                dist = dist.Remove(dist.IndexOf(' '));

                double.TryParse(dist, out toReturn);
            }
            catch
            {
                //Location was not found
                return -1;
            }
            return toReturn;
        }


        public class Rootobject
        {
            public Geocoded_Waypoints[] geocoded_waypoints { get; set; }
            public Route[] routes { get; set; }
            public string status { get; set; }
        }

        public class Geocoded_Waypoints
        {
            public string geocoder_status { get; set; }
            public string place_id { get; set; }
            public string[] types { get; set; }
        }

        public class Route
        {
            public Bounds bounds { get; set; }
            public string copyrights { get; set; }
            public Leg[] legs { get; set; }
            public Overview_Polyline overview_polyline { get; set; }
            public string summary { get; set; }
            public object[] warnings { get; set; }
            public object[] waypoint_order { get; set; }
        }

        public class Bounds
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }

        public class Northeast
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

        public class Southwest
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

        public class Overview_Polyline
        {
            public string points { get; set; }
        }

        public class Leg
        {
            public Distance distance { get; set; }
            public Duration duration { get; set; }
            public string end_address { get; set; }
            public End_Location end_location { get; set; }
            public string start_address { get; set; }
            public Start_Location start_location { get; set; }
            public Step[] steps { get; set; }
            public object[] traffic_speed_entry { get; set; }
            public object[] via_waypoint { get; set; }
        }

        public class Distance
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class Duration
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class End_Location
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

        public class Start_Location
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

        public class Step
        {
            public Distance1 distance { get; set; }
            public Duration1 duration { get; set; }
            public End_Location1 end_location { get; set; }
            public string html_instructions { get; set; }
            public Polyline polyline { get; set; }
            public Start_Location1 start_location { get; set; }
            public string travel_mode { get; set; }
            public string maneuver { get; set; }
        }

        public class Distance1
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class Duration1
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class End_Location1
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }

        public class Polyline
        {
            public string points { get; set; }
        }

        public class Start_Location1
        {
            public float lat { get; set; }
            public float lng { get; set; }
        }


    }
}
