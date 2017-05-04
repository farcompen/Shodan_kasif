using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using ShodanNET.Objects;

namespace ShodanNET
{
    public class Shodan
    {
        //
        private const string BaseUrl = " https://api.shodan.io/shodan/host/";
        private readonly string _apiKey;
        private JavaScriptSerializer _jsonParser = new JavaScriptSerializer();
        private WebClient _webClient = new WebClient();

        public Shodan(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        ///  Get all the information Shodan has on the IP.
        /// </summary>
        /// <param name="ip">IP of the computer to look up</param>
        /// <returns>A Host object with the banners and location information.</returns>
        public Host GetHost(IPAddress ip)
        {
            string strIp = ip.ToString();

            // Send the request
            Dictionary<string, string> args = new Dictionary<string, string>();
            args["ip"] = strIp;
            Dictionary<string, object> resDict = SendRequest("host", args);

            Host host = new Host(resDict);
            return host;
        }

        /// <summary>
        ///  Get all the information Shodan has on the IP (given as a string).
        /// </summary>
        /// <param name="ip">IP of the computer to look up</param>
        /// <returns>A Host object with the banners and location information.</returns>
        public Host GetHost(string ip)
        {
            return GetHost(IPAddress.Parse(ip));
        }

        /// <summary>
        ///  Search the Shodan search engine for computers matching the given search criteria.
        /// </summary>
        /// <param name="query">The search query for Shodan; identical syntax to the website. </param>
        /// <param name="offset">The starting position for the search cursor.</param>
        /// <param name="limit">The number of hosts to return per search query. Must be a multiple of 100.</param>
        /// <returns> A SearchResult object that contains a List of Hosts matching the query and the total number of results found. </returns>
        public List<Host> Search(string query, int offset = 0, int limit = 100)
        {
            if (limit % 100 != 0)
                throw new ArgumentException("Limit must be a multiple of 100.", "limit");

            Dictionary<string, string> args = new Dictionary<string, string>();
            args["query"] = query;
            args["o"] = offset.ToString();
            args["l"] = limit.ToString();
            Dictionary<string, object> resDict = SendRequest("search", args);

            List<Host> hosts = new List<Host>(resDict.Count);

            ArrayList arrayList = (ArrayList)resDict["matches"];
            foreach (Dictionary<string, object> item in arrayList)
            {
                hosts.Add(new Host(item, true));
            }

            return hosts;
        }

        public List<Exploit> SearchExploits(string query, string author = "", string platform = "", int port = 0, string type = "")
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args["query"] = query;

            if (!string.IsNullOrEmpty(author))
                args["author"] = author;

            if (!string.IsNullOrEmpty(platform))
                args["platform"] = platform;

            if (port >= 1)
                args["port"] = port.ToString();

            if (!string.IsNullOrEmpty(type))
                args["type"] = type;

            Dictionary<string, object> resDict = SendRequest("exploitdb/search", args);
            List<Exploit> exploits = new List<Exploit>(resDict.Count);

            ArrayList arrayList = (ArrayList)resDict["matches"];
            foreach (Dictionary<string, object> item in arrayList)
            {
                exploits.Add(new Exploit(item));
            }

            return exploits;
        }

        public DataResponse DownloadExploit(int id)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args["id"] = id.ToString();
            Dictionary<string, object> resDict = SendRequest("exploitdb/download", args);

            DataResponse exploit = new DataResponse(resDict);
            return exploit;
        }

        public List<MSFModule> SearchMSFModules(string query)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args["query"] = query;

            Dictionary<string, object> resDict = SendRequest("msf/search", args);
            List<MSFModule> modules = new List<MSFModule>(resDict.Count);

            ArrayList arrayList = (ArrayList)resDict["matches"];
            foreach (Dictionary<string, object> item in arrayList)
            {
                modules.Add(new MSFModule(item));
            }

            return modules;
        }

        public DataResponse DownloadMSFModule(string id)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            args["id"] = id;
            Dictionary<string, object> resDict = SendRequest("msf/download", args);

            DataResponse module = new DataResponse(resDict);
            return module;
        }

        /// <summary>
        ///  Internal wrapper function to send API requests.
        /// </summary>
        /// <param name="apiFunc">The API function to call.</param>
        /// <param name="args">The arguments to pass to the given API function.</param>
        private Dictionary<string, object> SendRequest(string apiFunc, Dictionary<string, string> args)
        {
            // Convert the arguments to a query string
            string strArgs = ToQuerystring(args);

            // Send the request
            Stream response = _webClient.OpenRead(BaseUrl + apiFunc + "?key="+ _apiKey+strArgs);
           

            if (response == null)
                return null;

            // Read the response into a string
            StreamReader reader = new StreamReader(response);
            string data = reader.ReadToEnd();
            reader.Close();

            // Turn the JSON string into a native dictionary object
            Dictionary<string, object> result = _jsonParser.Deserialize<Dictionary<string, object>>(data);

            // Raise an exception if an error was returned
            if (result.ContainsKey("error"))
                throw new ArgumentException((string)result["error"]);

            return result;
        }

        private string ToQuerystring(Dictionary<string, string> dict)
        {
            return "&" + string.Join("&", dict.Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x.Key), HttpUtility.UrlEncode(x.Value))));
        }
    }
}