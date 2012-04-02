using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VisuallyLocated.ArcGIS.Server
{
    public class ServiceExtension
    {
        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        [JsonProperty("capabilities")]
        public string Capabilities { get; set; }
        
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("properties")]
        public IDictionary<string, string> Properties { get; set; }
    }
}
