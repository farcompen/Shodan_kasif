using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShodanNET.Objects
{
    public class MSFModule
    {
        public MSFModule(Dictionary<string, object> data)
        {
            Alias = data["alias"] as string;
            Arch = data["arch"] as string;

            ArrayList authors = data["authors"] as ArrayList;
            if (authors != null)
                Authors = new List<string>(authors.Cast<string>());

            Description = data["description"].ToString();
            Fullname = data["fullname"].ToString();
            Name = data["name"].ToString();

            ArrayList platforms = data["platforms"] as ArrayList;
            if (platforms != null)
                Platforms = new List<string>(platforms.Cast<string>());

            Privileged = bool.Parse(data["privileged"].ToString());
            Rank = data["rank"] as string;

            ArrayList references = data["references"] as ArrayList;
            if (references != null)
            {
                References = new List<KeyValuePair<string, string>>();
                foreach (ArrayList reference in references)
                {
                    References.Add(new KeyValuePair<string, string>(reference[0].ToString(), reference[1].ToString()));
                }
            }

            Type = data["type"] as string;
            Version = data["version"] as string;
        }

        public string Alias { get; set; }
        public string Arch { get; set; }
        public List<string> Authors { get; set; }
        public string Description { get; set; }
        public string Fullname { get; set; }
        public string Name { get; set; }
        public List<string> Platforms { get; set; }
        public bool Privileged { get; set; }
        public string Rank { get; set; }
        public List<KeyValuePair<string, string>> References { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
    }
}