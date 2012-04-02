using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VisuallyLocated.ArcGIS.Server
{
    public static class WebRequestExtensions
    {
        public static Task<WebResponse> GetReponseAsync(this WebRequest request, Func<AsyncCallback, object, IAsyncResult> beginMethod = null, Func<IAsyncResult, WebResponse> endMethod = null)
        {
            if (beginMethod == null) beginMethod = request.BeginGetResponse;
            if (endMethod == null) endMethod = request.EndGetResponse;
            return Task.Factory.FromAsync<WebResponse>(
                            beginMethod,
                            endMethod,
                            null);
        }
    }
}
