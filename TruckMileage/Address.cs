using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace TruckMileage
{
    /// <summary>
    /// This class returns a Dictionary set of items to report an address from Google Maps and Places API. Also implements the IDisposable.
    /// </summary>
    public class Address : IDisposable
    {
        private string API_KEY = "";
        /// <summary>
        /// <paramref name="API_KEY"/> is needed for any functions to work called to work.
        /// </summary>
        /// <param name="API_KEY">The API KEY gotten from google.</param>
        public Address(string API_KEY)
        {
            this.API_KEY = API_KEY;
        }

        public void Dispose()
        {
            API_KEY = "";
        }

        /// <summary>
        /// Gets a formatted dictionary from the name of a place in the US using Google based off your own IP's location.
        /// </summary>
        /// <param name="nameOfEstablishment">Name of place you're looking for.</param>
        /// <returns>Keys: name, phone, address, city, state, zipcode</returns>
        public Dictionary<string, string> FindPlaceValues(string nameOfEstablishment)
        {
            string html = "https://maps.googleapis.com/maps/api/place/details/json?placeid=" + new Maps(API_KEY).FindPlaceId(nameOfEstablishment) + "&fields=name,formatted_address,formatted_phone_number&key=" + API_KEY;
            var client = new WebClient();
            var content = client.DownloadString(html);
            Rootobject pt = JsonConvert.DeserializeObject<Rootobject>(content);
            Dictionary<string, string> toReturn = new Dictionary<string, string>();
            toReturn.Add("name", pt.result.name);
            toReturn.Add("phone", pt.result.formatted_phone_number);
            string[] allComponents = pt.result.formatted_address.Split(',');
            string[] state_ZipCode;
            if (allComponents.Length > 0)
            {
                toReturn.Add("address", allComponents[0].Trim());
            }
            if (allComponents.Length > 1)
            {
                toReturn.Add("city", allComponents[1].Trim());
            }
            if (allComponents.Length > 2)
            {
                state_ZipCode = allComponents[2].Trim().Split(' ');
                if (state_ZipCode.Length > 0)
                {
                    toReturn.Add("state", state_ZipCode[0]);
                }
                if (state_ZipCode.Length > 1)
                {
                    toReturn.Add("zipcode", state_ZipCode[1]);
                }
            }
            
            return toReturn;
        }

        public class Rootobject
        {
            public object[] html_attributions { get; set; }
            public Result result { get; set; }
            public string status { get; set; }
        }

        public class Result
        {
            public string formatted_address { get; set; }
            public string formatted_phone_number { get; set; }
            public string name { get; set; }
        }

        public class Maps
        {
            private string API_KEY = "";

            /// <summary>
            /// <paramref name="API_KEY"/> is needed for any functions to work called to work.
            /// </summary>
            /// <param name="API_KEY">The API KEY gotten from google.</param>
            public Maps(string API_KEY)
            {
                this.API_KEY = API_KEY;
            }

            public string FindPlaceId(string nameOfEstablishment)
            {
                string html = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input=" + nameOfEstablishment + "&fields=place_id&inputtype=textquery&type=establishment&key=" + API_KEY;

                var client = new WebClient();
                var content = client.DownloadString(html);
                Rootobject pt = JsonConvert.DeserializeObject<Rootobject>(content);

                return pt.candidates[0].place_id;
            }

            public class Rootobject
            {
                public Candidate[] candidates { get; set; }
                public Debug_Log debug_log { get; set; }
                public string status { get; set; }
            }

            public class Debug_Log
            {
                public object[] line { get; set; }
            }

            public class Candidate
            {
                public string place_id { get; set; }
            }
        }
    }
}
