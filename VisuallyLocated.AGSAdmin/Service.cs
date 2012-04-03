/*
    Copyright (c) 2012 Shawn Kendrot
    This license governs use of the accompanying software. If you use the software, you
    accept this license. If you do not accept the license, do not use the software.

    Conditions and Limitations
    (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
    (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, 
        your patent license from such contributor to the software ends automatically.
    (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution
        notices that are present in the software.
    (D) If you distribute any portion of the software in source code form, you may do so only under this license by 
        including a complete copy of this license with your distribution. If you distribute any portion of the software 
        in compiled or object code form, you may only do so under a license that complies with this license.
    (E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, 
        guarantees or conditions. You may have additional consumer rights under your local laws which this license 
        cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of 
        merchantability, fitness for a particular purpose and non-infringement.
  
    For more information see http://www.microsoft.com/en-us/openness/licenses.aspx
*/

using System.Collections.Generic;
using Newtonsoft.Json;

namespace VisuallyLocated.ArcGIS.Server
{
    public class Service
    {
        [JsonProperty("serviceName")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public ServiceType Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("capabilities")]
        public string Capabilities { get; set; }

        [JsonProperty("clusterName")]
        public string ClusterName { get; set; }

        [JsonProperty("minInstancesPerNode")]
        public int MinInstancesPerNode { get; set; }

        [JsonProperty("maxInstancesPerNode")]
        public int MaxInstancesPerNode { get; set; }

        [JsonProperty("instancesPerContainer")]
        public int InstancesPerContainer { get; set; }

        [JsonProperty("maxWaitTime")]
        public int MaxWaitTime { get; set; }

        [JsonProperty("maxStartupTime")]
        public int MaxStartupTime { get; set; }

        [JsonProperty("maxIdleTime")]
        public int MaxIdleTime { get; set; }

        [JsonProperty("maxUsageTime")]
        public int MaxUsageTime { get; set; }

        [JsonProperty("loadBalancing")]
        public LoadBalancing LoadBalancing { get; set; }

        [JsonProperty("isolationLevel")]
        public IsolationLevel IsolationLevel { get; set; }

        [JsonProperty("configuredState")]
        public ServiceState ConfiguredState { get; set; }

        [JsonProperty("recycleInterval")]
        public int RecycleInterval { get; set; }

        [JsonProperty("recycleStartTime")]
        public string RecycleStartTime { get; set; }

        [JsonProperty("keepAliveInterval")]
        public int KeepAliveInterval { get; set; }

        [JsonProperty("private")]
        public bool Private { get; set; }

        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }

        [JsonProperty("properties")]
        public IDictionary<string, string> Properties { get; set; }

        [JsonProperty("extensions")]
        public IEnumerable<ServiceExtension> Extensions { get; set; }

        [JsonProperty("datasets")]
        public IEnumerable<object> Datasets { get; set; }

    }
}