using System;

namespace ShodanNET.Objects
{
    public class ServiceBanner
    {
        public ServiceBanner(int argPort, string argBanner, DateTime argTimestamp)
        {
            Port = argPort;
            Banner = argBanner;
            Timestamp = argTimestamp;
        }

        public int Port { get; private set; }
        public string Banner { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}