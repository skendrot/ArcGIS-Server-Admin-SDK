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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace VisuallyLocated.ArcGIS.Server
{
    public class Admin
    {
        private readonly string _serverUrl;

        /// <summary>
        /// Initializes a new Admin. 
        /// </summary>
        /// <param name="serverUrl">The url to the server to administer. 
        /// This value should include the full url to get to the ArcGIS Server, included http and port number.</param>
        /// <exception cref="ArgumentNullException">The url cannot be null.</exception>
        /// <example>
        /// <code>
        /// Admin admin = new Admin("http://localhost:6080");
        /// </code>
        /// </example>
        public Admin(string serverUrl)
        {
            if (string.IsNullOrEmpty(serverUrl)) throw new ArgumentNullException("serverUrl");

            _serverUrl = serverUrl;
        }

        /// <summary>
        /// Generates a new <see cref="UserToken"/>
        /// </summary>
        /// <param name="user">The ArcGIS Server Manager user name to use.</param>
        /// <param name="password">The passord for the given user name.</param>
        /// <returns>The <see cref="UserToken"/> for the given name and password.</returns>
        /// <exception cref="ArgumentNullException">User name cannot be null.</exception>
        /// <exception cref="ArgumentNullException">Password cannot be null.</exception>
        public Task<UserToken> GenerateTokenAsync(string user, string password)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (password == null) throw new ArgumentNullException("password");

            var parameters = GetBaseParameters();
            parameters.Add(Constants.UserName, user);
            parameters.Add(Constants.Password, password);
            parameters.Add(Constants.Client, Constants.RequestIP);

            var taskCompletionSource = new TaskCompletionSource<UserToken>();
            RequestResult(parameters, Constants.TokenUrl,
                result => taskCompletionSource.SetResult(JsonConvert.DeserializeObject<UserToken>(result)),
                true);
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Retrieves the machines associated with the ArcGIS Server.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        public Task<IEnumerable<Machine>> GetMachinesAsync(UserToken token)
        {
            if (token == null) throw new ArgumentNullException("token");

            var parameters = GetBaseParameters(token);

            var taskCompletionSource = new TaskCompletionSource<IEnumerable<Machine>>();
            RequestResult(parameters, Constants.MachinesUrl,
                result => taskCompletionSource.SetResult(JsonConvert.DeserializeObject<IEnumerable<Machine>>(result)));
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Retrieves the service from the ArcGIS Server under the folder with the given name.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <param name="serviceName">The name of the service to get.</param>
        /// <param name="serviceType">The <see cref="ServiceType"/> of the service.</param>
        /// <param name="folder">The folder the service in under. If no folder is given,
        /// will attempt to get the service under the root folder.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        /// <exception cref="ArgumentNullException">serviceName cannot be null.</exception>
        public Task<Service> GetServiceAsync(UserToken token, string serviceName, ServiceType serviceType, string folder = null)
        {
            if (token == null) throw new ArgumentNullException("token");
            if (string.IsNullOrEmpty(serviceName)) throw new ArgumentNullException("serviceName");

            var parameters = GetBaseParameters(token);

            string url = GetServiceTypeUrl(GetFolderUrl(Constants.ServicesUrl, folder), serviceName, serviceType);

            var taskCompletionSource = new TaskCompletionSource<Service>();
            RequestResult(parameters, url,
                result => taskCompletionSource.SetResult(JsonConvert.DeserializeObject<Service>(result)));
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Retrieves al of the services under the given folder. If no folder is given, 
        /// will retrieve services from the root folder.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <param name="getFulldetails">A value indicating whether all details for the services should be returned.</param>
        /// <param name="folder">The folder under which to get services. If no folder is given,
        /// will get services under the root folder.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        public Task<ServicesContainer> GetServicesAsync(UserToken token, bool getFulldetails, string folder = null)
        {
            if (token == null) throw new ArgumentNullException("token");

            var parameters = GetBaseParameters(token);
            parameters.Add(Constants.Detail, getFulldetails.ToString(CultureInfo.InvariantCulture));

            var taskCompletionSource = new TaskCompletionSource<ServicesContainer>();
            RequestResult(parameters, GetFolderUrl(Constants.ServicesUrl, folder),
                result => taskCompletionSource.SetResult(JsonConvert.DeserializeObject<ServicesContainer>(result)));
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Creates a new folder under the root directory in the server.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <param name="folderName">The name of the folder to create.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        /// <exception cref="ArgumentNullException">folderName cannot be null.</exception>
        public Task<RequestStatus> CreateFolderAsync(UserToken token, string folderName)
        {
            if (token == null) throw new ArgumentNullException("token");
            if (string.IsNullOrEmpty(folderName)) throw new ArgumentNullException("folderName");

            var parameters = GetBaseParameters(token);
            parameters.Add(Constants.FolderName, folderName);
            parameters.Add(Constants.Description, "None");

            var taskCompletionSource = new TaskCompletionSource<RequestStatus>();
            RequestResult(parameters, Constants.CreateFolderUrl,
                result => taskCompletionSource.SetResult(JsonConvert.DeserializeObject<RequestStatus>(result)),
                true);
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Posts the service to the server.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <param name="service">The service to post.</param>
        /// <param name="folder">The folder the service is contained within. 
        /// If no folder is given, will attempt to post the service to the root folder.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        /// <exception cref="ArgumentNullException">service cannot be null.</exception>
        public Task<RequestStatus> EditServiceAsync(UserToken token, Service service, string folder = null)
        {
            if (token == null) throw new ArgumentNullException("token");
            if (service == null) throw new ArgumentNullException("service");

            var parameters = GetBaseParameters(token);
            var taskCompletionSource = new TaskCompletionSource<RequestStatus>();

            string url = GetServiceTypeUrl(GetFolderUrl(Constants.ServicesUrl, folder), service.Name, service.Type);
            RequestResult(parameters, string.Format("{0}{1}", url, Constants.EditUrl), 
                result => taskCompletionSource.SetResult(RequestStatus.Success), true,
                "service=" + JsonConvert.SerializeObject(service));
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Get all of the items uploaded to the server.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        public Task<IEnumerable<UploadedItem>> GetUploadedItems(UserToken token)
        {
            if (token == null) throw new ArgumentNullException("token");

            var parameters = GetBaseParameters(token);
            var taskCompletionSource = new TaskCompletionSource<IEnumerable<UploadedItem>>();

            RequestResult(parameters, Constants.UploadsUrl,
                result => taskCompletionSource.SetResult(JsonConvert.DeserializeObject<IEnumerable<UploadedItem>>(result)));
            return taskCompletionSource.Task;          
        }

        /// <summary>
        /// Upload an item to the server.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <param name="parameters">The parameters of the item to upload.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        /// <exception cref="ArgumentNullException">parameters cannot be null.</exception>
        public Task<UploadResult> UploadItemAsync(UserToken token, UploadParameters parameters)
        {
            if (token == null) throw new ArgumentNullException("token");
            if (parameters == null) throw new ArgumentNullException("parameters");

            var taskCompletionSource = new TaskCompletionSource<UploadResult>();
            var uploader = new UploadFormDataRequest(token, parameters);
            uploader.UploadAsync(_serverUrl,
                result => taskCompletionSource.SetResult(JsonConvert.DeserializeObject<UploadResult>(result)));
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Registers a new server object extension file with the Server. Before your register the file, 
        /// you need to upload the .SOE file to the server using <see cref="M:UploadItemAsync"/>.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <param name="id">The id of the extension to register.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        /// <exception cref="ArgumentNullException">id cannot be null.</exception>
        public Task<RequestStatus> RegisterExtensionAsync(UserToken token, string id)
        {
            if (token == null) throw new ArgumentNullException("token");
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException("id");

            var parameters = GetBaseParameters(token);
            parameters[Constants.ID] = id;

            var taskCompletionSource = new TaskCompletionSource<RequestStatus>();
            RequestResult(parameters, Constants.RegisterUrl,
                result => taskCompletionSource.SetResult(JsonConvert.DeserializeObject<RequestStatus>(result)),
                true);
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Stops all services in the given folder. If no folder if given, will stop all services under the root folder.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <param name="folder">The folder under which all services should be stopped. If not folder is given,
        /// will stop all services under the root folder.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        public Task<RequestStatus> StopServicesAsync(UserToken token, string folder = null)
        {
            if (token == null) throw new ArgumentNullException("token");

            return StartOrStopServicesAsync(token, Constants.Stop, folder);
        }

        /// <summary>
        /// Starts all services in the given folder. If no folder if given, will start all services under the root folder.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <param name="folder">The folder under which all services should be started. If not folder is given,
        /// will start all services under the root folder.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        public Task<RequestStatus> StartServicesAsync(UserToken token, string folder = null)
        {
            if (token == null) throw new ArgumentNullException("token");

            return StartOrStopServicesAsync(token, Constants.Start, folder);
        }

        /// <summary>
        /// Start the given service on all registered machines.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <param name="serviceName">The name of the service to start.</param>
        /// <param name="serviceType">The type of the service to start.</param>
        /// <param name="folder">The folder the service is under. If no folder is given, 
        /// will attempt to stop the service under the root folder.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        /// <exception cref="ArgumentNullException">serviceName cannot be null.</exception>
        public Task<RequestStatus> StartServiceAsync(UserToken token, string serviceName, ServiceType serviceType, string folder = null)
        {
            if (token == null) throw new ArgumentNullException("token");
            if (string.IsNullOrEmpty(serviceName)) throw new ArgumentNullException("serviceName");

            return StartOrStopServiceAsync(serviceName, serviceType, token, Constants.Start, folder);
        }

        /// <summary>
        /// Stop the services on all registered machines.
        /// </summary>
        /// <param name="token">The <see cref="UserToken"/> for the ArcGIS Server Manager user.</param>
        /// <param name="serviceName">The name of the service to stop.</param>
        /// <param name="serviceType">The type of the service to stop.</param>
        /// <param name="folder">The folder the service is under. If no folder is given, 
        /// will attempt to start the service under the root folder.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">token cannot be null.</exception>
        /// <exception cref="ArgumentNullException">serviceName cannot be null.</exception>
        public Task<RequestStatus> StopServiceAsync(UserToken token, string serviceName, ServiceType serviceType, string folder = null)
        {
            if (token == null) throw new ArgumentNullException("token");
            if (string.IsNullOrEmpty(serviceName)) throw new ArgumentNullException("serviceName");

            return StartOrStopServiceAsync(serviceName, serviceType, token, Constants.Stop, folder);
        }

        private Task<RequestStatus> StartOrStopServicesAsync(UserToken token, string action, string folder = null)
        {
            var taskCompletionSource = new TaskCompletionSource<RequestStatus>();
            GetServicesAsync(token, false, folder)
                .ContinueWith(
                servicesTask =>
                {
                    var services = servicesTask.Result.Services.ToList();
                    int serviceCount = services.Count;
                    foreach (var service in services)
                    {
                        StartOrStopServiceAsync(service.Name, service.Type, token, action, folder)
                            .ContinueWith(t =>
                                              {
                                                  // TODO: Need a thread safe way to do this...
                                                  serviceCount--;
                                                  if (serviceCount == 0)
                                                      taskCompletionSource.SetResult(RequestStatus.Success);
                                              });
                    }
                });
            return taskCompletionSource.Task;
        }

        private Task<RequestStatus> StartOrStopServiceAsync(string serviceName, ServiceType serviceType, UserToken token, string action, string folder)
        {
            var parameters = GetBaseParameters(token);

            string url = GetServiceTypeUrl(GetFolderUrl(Constants.ServicesUrl, folder), serviceName, serviceType);

            var taskCompletionSource = new TaskCompletionSource<RequestStatus>();
            RequestResult(parameters, string.Format("{0}/{1}/", url, action),
                result => taskCompletionSource.SetResult(JsonConvert.DeserializeObject<RequestStatus>(result)));
            return taskCompletionSource.Task;
        }

        private void RequestResult(IDictionary<string, string> parameters, string adminEndpoint, Action<string> callback, bool post = false, string postData = null)
        {
            var uri = new Uri(string.Format("{0}{1}?{2}", _serverUrl, adminEndpoint, GetQueryString(parameters)), UriKind.Absolute);

            WebRequest webRequest = WebRequest.Create(uri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = post ? "POST" : "GET";
            //webRequest.Accept = "text/plain";

            webRequest.BeginGetRequestStream(ar =>
            {
                if (postData != null)
                {
                    var encoding = new UTF8Encoding();
                    var bytes = encoding.GetBytes(postData);
                    using (Stream os = webRequest.EndGetRequestStream(ar))
                    {
                        os.Write(bytes, 0, bytes.Length);
                    }
                }
                webRequest.BeginGetResponse(
                    ar2 =>
                    {
                        using (var response = webRequest.EndGetResponse(ar2))
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            string result = reader.ReadToEnd();
                            if (callback != null) callback(result);
                        }
                    }, null);
            }, null);
        }

        private static IDictionary<string, string> GetBaseParameters(UserToken token = null)
        {
            var parameters = new Dictionary<string, string>();
            if (token != null)
            {
                parameters.Add(Constants.Token, token.Token);
            }
            parameters.Add(Constants.Format, Constants.Json);
            return parameters;
        }

        private static string GetFolderUrl(string baseUrl, string folder)
        {
            string slash = string.IsNullOrEmpty(folder) ? "" : "/";
            return string.Format("{0}{1}{2}", baseUrl, folder, slash);
        }

        private static string GetServiceTypeUrl(string baseUrl, string serviceName, ServiceType serviceType)
        {
            return string.Format("{0}{1}.{2}", baseUrl, serviceName, serviceType);
        }

        private static string GetQueryString(IDictionary<string, string> parameters)
        {
            return string.Join("&", parameters.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)).ToArray());
        }
    }
}