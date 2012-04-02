using System;
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