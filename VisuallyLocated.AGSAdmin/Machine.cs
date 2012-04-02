using System.Collections.Generic;

namespace VisuallyLocated.ArcGIS.Server
{
    public class Machine
    {
        public string MachineName { get; set; }
        public string Platform { get; set; }
        public int WebServerMaxHeapSize { get; set; }
        public int AppServerMaxHeapSize { get; set; }
        public bool WebServerSslEnabled { get; set; }
        public string AdminUrl { get; set;  }
        public IDictionary<string, int> Ports { get; set; }
        public int SocMaxHeapSize { get; set; }
        /*
         *  "ServerStartTime":1332971232715,
         *  "webServerCertificateAlias":"SelfSignedCertificate",
         */
    }
}