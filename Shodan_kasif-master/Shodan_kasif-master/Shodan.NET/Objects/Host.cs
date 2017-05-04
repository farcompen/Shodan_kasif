using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace ShodanNET.Objects
{
    public class Host
    {
        public Host(Dictionary<string, object> host, bool simple = false)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            // Extract the info out of the host dictionary and put it in the local properties
            IP = IPAddress.Parse(host["ip"].ToString());

          /*  if (host.ContainsKey("os"))
                OS = host["os"].ToString();
            else
                OS = "Unknown";*/

            // Hostnames
            ArrayList tmp = (ArrayList)host["hostnames"];
            Hostnames = tmp.Cast<string>().ToList();

            // Banners
            Banners = new List<ServiceBanner>();

            if (host["data"] is ArrayList)
            {
                tmp = (ArrayList)host["data"];
                foreach (Dictionary<string, object> data in tmp)
                {
                    DateTime timestamp = DateTime.ParseExact((string)data["timestamp"], "dd.MM.yyyy", provider);
                    Banners.Add(new ServiceBanner((int)data["port"], (string)data["banner"], timestamp));
                }
            }
            else if (host["data"] is string)
            {
             //   DateTime timestamp = DateTime.ParseExact((string)host["timestamp"], "yyyy-MM-dd", provider);
             //   Banners.Add(new ServiceBanner((int)host["port"], (string)host["data"], timestamp));
            }

            // Location
            Location = new HostLocation(host);

            IsSimple = simple;
        }

        public List<ServiceBanner> Banners { get; private set; }
        public   IPAddress IP { get; private set; }
        public List<string> Hostnames { get; private set; }
        public HostLocation Location { get; private set; }
        public string OS { get; private set; }

        /// <summary>
        /// Used to differentiate between hosts from Search() results and direct GetHost() queries
        /// </summary>
        public bool IsSimple { get; private set; }
    }
}