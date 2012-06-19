using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisuallyLocated.ArcGIS.Server
{
    internal static class Extensions
    {
        internal  static void SetAdminResult<T>(this TaskCompletionSource<T> source, AdminResult<T> result)
        {
            if (source == null) return;
            if (result == null) return;

            if (result.Exception != null)
            {
                source.SetException(result.Exception);
            }
            else
            {
                source.SetResult(result.Result);
            }
        }
    }
}
