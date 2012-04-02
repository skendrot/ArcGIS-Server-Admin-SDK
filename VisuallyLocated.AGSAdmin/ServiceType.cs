using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VisuallyLocated.ArcGIS.Server
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ServiceType
    {
        MapServer,
        FeatureService,
        GeometryServer,
        SearchServer
    }
}