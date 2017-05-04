using System.Collections.Generic;

namespace ShodanNET.Objects
{
    public class HostLocation
    {
        public HostLocation(Dictionary<string, object> host)
        {
            // Extract the info out of the host dictionary and put it in the local properties
            if (host.ContainsKey("country_name"))
                CountryName = (string)host["country_name"];

            if (host.ContainsKey("country_code"))
                CountryCode = (string)host["country_code"];

            if (host.ContainsKey("city"))
                City = (string)host["city"];

            if (host.ContainsKey("latitude"))
            {
                Latitude = (double)((decimal)host["latitude"]);
                Longitude = (double)((decimal)host["longitude"]);
            }
        }

        public string CountryCode { get; private set; }
        public string CountryName { get; private set; }
        public string City { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        /// <summary>
        ///   Check whether there are valid coordinates available for this location.
        /// </summary>
        /// <returns> true if there are latitude/ longitude coordinates, false otherwise. </returns>
        public bool HasCoordinates()
        {
            if (Latitude != 0 && Longitude != 0)
                return true;

            return false;
        }

        public override string ToString()
        {
            string output = string.Empty;

            if (!string.IsNullOrEmpty(City))
                output += City;

            if (!string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(CountryName))
                output += ", ";

            if (!string.IsNullOrEmpty(CountryName))
                output += CountryName;

            if (HasCoordinates())
                output += " at " + Latitude + "," + Longitude;

            return output;
        }
    }
}