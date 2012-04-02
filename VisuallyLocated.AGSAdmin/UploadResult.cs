using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisuallyLocated.ArcGIS.Server
{
    public class UploadResult
    {
        public RequestStatus Status { get; set; }
        public UploadedItem Item { get; set; }
    }
}
