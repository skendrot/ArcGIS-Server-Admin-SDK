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

namespace VisuallyLocated.ArcGIS.Server
{
    static class Constants
    {
        private const string AdminUrl = "/arcgis/admin/";

        public const string PublicKeyUrl = AdminUrl + "publicKey";
        public const string TokenUrl = AdminUrl + "generateToken";
        public const string ServicesUrl = AdminUrl + "services/";
        public const string QueryUrl = AdminUrl + "logs/query";
        public const string CreateFolderUrl = AdminUrl + "services/createFolder/";
        public const string MachinesUrl = AdminUrl + "machines";
        public const string UploadsUrl = AdminUrl + "uploads/";
        public const string UploadItemUrl = UploadsUrl + "upload";
        public const string RegisterUrl = AdminUrl + "services/types/extensions/register";
        public const string EditUrl = "/edit";

        public const string UserName = "username";
        public const string Password = "password";
        public const string Client = "client";
        public const string Format = "f";
        public const string Token = "token";
        public const string Json = "json";
        public const string Level = "level";
        public const string Filter = "filter";
        public const string Service = "service";
        public const string Services = "services";
        public const string Detail = "detail";
        public const string FolderName = "folderName";
        public const string Description = "description";
        public const string Start = "start";
        public const string Stop = "stop";
        public const string ID = "id";
        public const string RequestIP = "requestip";
        public const string Encrypted = "encrypted";
        public const string True = "true";
    }
}
