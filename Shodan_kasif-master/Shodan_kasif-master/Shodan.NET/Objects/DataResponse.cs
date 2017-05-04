using System.Collections.Generic;
using System.IO;

namespace ShodanNET.Objects
{
    public class DataResponse
    {
        public DataResponse(Dictionary<string, object> data)
        {
            Data = data["data"] as string;
            ContentType = data["content-type"] as string;
            Filename = data["filename"] as string;
        }

        public string Data { get; set; }
        public string ContentType { get; set; }
        public string Filename { get; set; }

        public void WriteToFile(string path)
        {
            if (!Directory.Exists(path))
            {
                string directoryName = Path.GetDirectoryName(path);

                if (directoryName != null)
                    Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(path, Data);
        }
    }
}