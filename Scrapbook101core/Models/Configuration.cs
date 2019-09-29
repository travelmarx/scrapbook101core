using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrapbook101core
{
    /// <summary>
    /// Represents the application settings defined in <c>appsettings.json.</c>
    /// </summary>
    public class Scrapbook101Configuration
    {
        public static Scrapbook101Configuration Current;

        public Scrapbook101Configuration()
        {
            Current = this;
        }

        public bool AddTestAssets { get; set; }
        public string ApplicationName { get; set; }
        public string AssetBasePath { get; set; }
        public string AuthKey { get; set; }
        public string BingMapKey { get; set; }
        public string CategoryDocumentType { get; set; }
        public string CollectionId { get; set; }
        public string DatabaseId { get; set; }
        public string Endpoint { get; set; }
        public string ItemDocumentType { get; set; }
        public string NoAssetImage { get; set; }
        public string TestCategories { get; set; }
        public string TestDocuments { get; set; }
    }
}
