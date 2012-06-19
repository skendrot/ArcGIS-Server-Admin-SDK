using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisuallyLocated.ArcGIS.Server
{
    public class AdminResult<T>
    {
        public RequestStatus Status { get; set; }

        public Exception Exception { get; internal set; }

        public T Result { get; set; }
    }
}
