using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VisuallyLocated.ArcGIS.Server
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LogLevel
    {
        Severe,
        Warning,
        Info,
        Fine,
        Debug,
        Verbose
    }
}
