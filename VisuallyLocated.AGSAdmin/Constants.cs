using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisuallyLocated.ArcGIS.Server
{
    static class Constants
    {
        private const string AdminUrl = "/arcgis/admin/";

        public const string TokenUrl = AdminUrl + "generateToken";
        public const string ServicesUrl = AdminUrl + "services/";
        public const string QueryUrl = AdminUrl + "logs/query";
        public const string CreateFolderUrl = AdminUrl + "services/createFolder/";
        public const string MachinesUrl = AdminUrl + "machines";
        public const string UploadUrl = AdminUrl + "uploads/upload";
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
    }
}
