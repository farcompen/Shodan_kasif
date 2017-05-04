using System;
using System.Collections.Generic;
using System.Configuration;
using ShodanNET;
using ShodanNET.Objects;

namespace ShodanNETClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // --> Insert your App Key in the app.config file <--
            Shodan shodan = new Shodan(ConfigurationManager.AppSettings["AppKey"]);

            //Print a list of cisco-ios devices
            List<Host> hosts = shodan.Search("cisco-ios");

            foreach (Host h in hosts)
            {
                Console.WriteLine(h.IP.ToString());
            }

            //Get all the information SHODAN has on the IP 217.140.75.46
            Host host = shodan.GetHost("217.140.75.46");
            Console.WriteLine(host.IP.ToString());

            //Search for exploits on ExploitDB and modules on MSF
            List<Exploit> exploits = shodan.SearchExploits("Microsoft Windows XP");

            foreach (Exploit exploit in exploits)
            {
                Console.WriteLine(exploit.Description);
            }

            List<MSFModule> modules = shodan.SearchMSFModules("Oracle");

            foreach (MSFModule msfModule in modules)
            {
                Console.WriteLine(msfModule.Name);
            }

            //Download exploit from ExploitDB and modules from MSF - Currently disabled by ShodanHQ
            //DataResponse exploitData = shodan.DownloadExploit(exploits[0].Id); 
            //Console.WriteLine(exploitData.Filename);

            //DataResponse module = shodan.DownloadMSFModule("exploit/windows/browser/ms06_055_vml_method");

            //Note that we also write the file to disk
            //module.WriteToFile("C:\\" + module.Filename);
            //Console.WriteLine(module.Filename);

            Console.WriteLine("Press a key to continue.");
            Console.ReadLine();
        }
    }
}
