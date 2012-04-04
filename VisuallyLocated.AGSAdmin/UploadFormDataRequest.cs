/*
    Copyright (c) 2012 Shawn Kendrot
    This license governs use of the accompanying software. If you use the software, you
    accept this license. If you do not accept the license, do not use the software.

    Conditions and Limitations
    (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
    (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, 
        your patent license from such contributor to the software ends automatically.
    (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution
        notices that are present in the software.
    (D) If you distribute any portion of the software in source code form, you may do so only under this license by 
        including a complete copy of this license with your distribution. If you distribute any portion of the software 
        in compiled or object code form, you may only do so under a license that complies with this license.
    (E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, 
        guarantees or conditions. You may have additional consumer rights under your local laws which this license 
        cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of 
        merchantability, fitness for a particular purpose and non-infringement.
  
    For more information see http://www.microsoft.com/en-us/openness/licenses.aspx
*/

using System;
using System.Globalization;
using System.IO;
using System.Net;

namespace VisuallyLocated.ArcGIS.Server
{
    internal class UploadFormDataRequest : WebRequest
    {
        private readonly UserToken _token;
        private readonly UploadParameters _parameters;
        private readonly string _boundary;
        private WebRequest _webRequest;

        public UploadFormDataRequest(UserToken token, UploadParameters parameters)
        {
            _token = token;
            _parameters = parameters;
            _boundary = string.Format("---------------------------{0}",
                DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture).Substring(0, 12));
        }

        public void UploadAsync(string serverUrl, Action<string> callback)
        {
            var uri = new Uri(string.Format("{0}{1}?token={2}", serverUrl, Constants.UploadItemUrl, _token.Token), UriKind.Absolute);

            _webRequest = Create(uri);
            ((HttpWebRequest)_webRequest).AllowWriteStreamBuffering = true;
            _webRequest.Method = "POST";
            _webRequest.ContentType = "multipart/form-data; boundary=" + _boundary;
            _webRequest.BeginGetRequestStream(asyncResult => GetValue(asyncResult, callback), null);
        }

        private void GetValue(IAsyncResult requestStream, Action<string> callback)
        {
            using (var stream = _webRequest.EndGetRequestStream(requestStream))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("--");
                    writer.Write(_boundary);

                    writer.WriteLine();
                    writer.WriteLine(string.Format(
                        "Content-Disposition: form-data; name=\"itemFile\"; filename=\"{0}\"",
                        Uri.EscapeDataString(_parameters.FileName)));
                    writer.WriteLine();
                    writer.Flush();
                    var buffer = new byte[_parameters.FileStream.Length];
                    _parameters.FileStream.Read(buffer, 0, buffer.Length);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                    writer.WriteLine();
                    writer.Write("--");
                    writer.Write(_boundary);
                    writer.Flush();

                    if (string.IsNullOrEmpty(_parameters.Description) == false)
                    {
                        writer.WriteLine();
                        writer.WriteLine("Content-Disposition: form-data; name=\"description\"");
                        writer.WriteLine();
                        writer.Flush();
                        writer.WriteLine(_parameters.Description);
                        writer.Write("--");
                        writer.Write(_boundary);
                        writer.Flush();
                    }

                    writer.WriteLine();
                    writer.WriteLine("Content-Disposition: form-data; name=\"f\"");
                    writer.WriteLine();
                    writer.Flush();
                    writer.WriteLine("json");
                    writer.Write("--");
                    writer.Write(_boundary);
                    writer.Flush();

                    // end
                    writer.WriteLine("--");
                    writer.Flush();
                }
            }
            _webRequest.BeginGetResponse(
                    ar2 =>
                    {
                        using (var response = _webRequest.EndGetResponse(ar2))
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            string result = reader.ReadToEnd();
                            if (callback != null) callback(result);
                        }
                    }, null);
        }


    }
}
