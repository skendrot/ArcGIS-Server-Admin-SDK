using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VisuallyLocated.ArcGIS.Server
{
    public class UploadParameters
    {
        public string FileName { get; set; }
        public string Description { get; set; }
        public Stream FileStream { get; set; }
    }
}
