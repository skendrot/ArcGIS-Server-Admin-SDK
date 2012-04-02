using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisuallyLocated.ArcGIS.Server
{
    public class ServicesContainer
    {
        public string FolderName { get; set; }
        public string Description { get; set; }
        public bool WebEncrypted { get; set; }
        public bool IsDefault { get; set; }
        public IEnumerable<string> Folders { get; set; }
        public IEnumerable<Service> Services { get; set; }
    }
}
