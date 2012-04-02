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
            var uri = new Uri(string.Format("{0}{1}?token={2}", serverUrl, Constants.UploadUrl, _token.Token), UriKind.Absolute);

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
