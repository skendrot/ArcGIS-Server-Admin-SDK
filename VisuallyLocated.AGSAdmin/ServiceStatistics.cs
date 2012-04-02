using System.Collections.Generic;

namespace VisuallyLocated.ArcGIS.Server
{
    public class ServiceStatistics
    {
        public ServiceStatisticsSummary Summary { get; set; }
        public IEnumerable<ServiceStatisticsPerMachine> PerMachine { get; set; }
    }
}