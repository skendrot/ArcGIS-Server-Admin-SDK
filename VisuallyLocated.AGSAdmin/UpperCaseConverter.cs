using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VisuallyLocated.ArcGIS.Server
{
    public class UpperCaseStringEnumConverter : StringEnumConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(value == null)
            {
                
            }
            else
            {
                writer.WriteValue(value.ToString().ToUpper());
            }
        }
    }
}
