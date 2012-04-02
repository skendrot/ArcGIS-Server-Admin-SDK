
namespace VisuallyLocated.ArcGIS.Server
{
    public class ServiceStatisticsSummary
    {
        public string FolderName { get; set; }
        public string ServiceName { get; set; }
        public ServiceType Type { get; set; }
        public string StartTime { get; set; }
        public int Max { get; set; }
        public int Busy { get; set; }
        public int Free { get; set; }
        public int Initializing { get; set; }
        public int NotCreated { get; set; }
        public int Transactions { get; set; }
        public int TotalBusyTime { get; set; }
        public bool IsStatisticsAvailable { get; set; }
    }
}
