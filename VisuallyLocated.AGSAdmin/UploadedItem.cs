namespace VisuallyLocated.ArcGIS.Server
{
    public class UploadedItem
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string PathOnServer { get; set; }
        public bool Committed { get; set; }
    }
}