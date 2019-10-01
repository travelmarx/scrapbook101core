namespace Scrapbook101core.Models
{
    using Microsoft.Azure.Documents.Spatial;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the main Scrapbook item or entry.
    /// </summary>
    /// <remarks>
    /// A Scrapbook Item object when written to the Document DB bcomes a block of JSON-formatted text.
    /// Not all properties are required.
    /// </remarks>
    public class Item
    {
        /// <value>
        /// A list of assets associated with the item. Can be null, meaning no assets are associated with the item.
        /// </value>
        [JsonProperty(PropertyName = "assets")]
        public List<AssetItem> Assets { get; set; }

        /// <value>
        /// This is the path to any digital assets associated with the item. Example value: "assets/2019/2019-01-01".
        /// </value>
        /// <remarks>
        /// For example, there might be images, PDFs
        /// or other documents that give context about the item. Associating assets is not necessary if you plan to
        /// implement Scrapbook101core without any assets. Part of the spirit behind Scrapbook101core however is
        /// to provide strong visual cues for remembering an item as well as archiving data associated with it,
        /// so at least one image for each Scrapbook101core is recommended. 
        /// </remarks>
        [JsonProperty(PropertyName = "assetPath")]
        public string AssetPath { get; set; }

        /// <value>
        /// One of the category values described in the <see cref="Scrapbook101core.Models.Category"/> class.
        /// A Scrapbook101core item must be assigned to a category.
        /// </value>
        [Required(ErrorMessage = "Category is required.")]
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        /// <value>
        /// Fields specific to the category selected for the Scrapbook101core item. 
        /// See the <see cref="Scrapbook101core.Models.Category"/> class for more information.
        /// Each category has a fixed set of fields.
        /// </value>
        [JsonProperty(PropertyName = "categoryFields")]
        public CategoryFields CategoryFields { get; set; }

        /// <value>
        /// This value is automatically inserted by code upon initial creation of an item and then never changed.
        /// Example: A string that is an ISO 8601 date and time, e.g., "2018-06-07T00:00:00".
        /// </value>
        [JsonProperty(PropertyName = "dateAdded")]
        public DateTime DateAdded { get; set; }

        /// <value>
        /// This value is automatically inserted.
        /// Example: A string that is an ISO 8601 date and time, e.g., "2018-06-07T00:00:00".
        /// </value>
        /// <remarks>
        /// On initial creation of an Scrapbook101core item,
        /// this property equals <b>dateAdded</b>. When an item is edited, this property is updated with the correct timestamp.
        /// </remarks>
        [JsonProperty(PropertyName = "dateUpdated")]
        public DateTime DateUpdated { get; set; }

        /// <value>
        /// Information about the Scrapbook101core item that isn't already captured in other fields.
        /// </value>
        /// <remarks>
        /// This property provides the
        /// context of why the item is important. Descriptions longer than a couple hundred words should be added as assets.
        /// </remarks>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <value>
        /// Represents a point geometry class in the Azure Cosmos DB service.
        /// </value>
        /// <remarks>
        /// A <a href="https://docs.microsoft.com/dotnet/api/microsoft.azure.documents.spatial.point">Microsoft.Azure.Documents.Spatial.Point</a>
        /// If you have a Bing Maps Key and it is specified in the appsettings.json file, then location values are
        /// converted into latitude and longitude coordinates (geocoded) to allow for more flexible location searches.
        /// </remarks>
        [JsonProperty(PropertyName = "geoLocation")]
        public Point GeoLocation { get; set; }

        /// <value>
        /// An auto-generated globally unique identifier (GUID), like "c73af369-7a21-45dd-b9ec-fe31af354fe1".
        /// </value>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <value>
        /// A friendly name of the location that is relevant for the Scrapbook101core item.
        /// </value>
        /// <remarks>
        /// It is not required but is helpful. It is important to be consistent with how you enter values to make searching easier.
        /// Examples: "Seattle, WA" or "101 Main St. Seattle 98103, USA" or "Italy".
        /// </remarks>
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        /// <value>
        /// A rating of the Scrapbook101core item, if applicable.
        /// </value>
        /// <remarks>
        /// The rating system will be unique for each implementation. The test data provided with Scrapbook101core
        /// uses a rating of 1 - 5.
        /// </remarks>
        [JsonProperty(PropertyName = "rating")]
        public string Rating { get; set; }

        /// <value>
        /// An identifier representing who changed the entry.
        /// </value>
        /// <remarks>
        /// This could be a user name or ID. For example, if you implement
        /// an authentication scheme based on Windows Live, Facebook, or Google, you can use the user's email or ID in this field.
        /// </remarks>
        [JsonProperty(PropertyName = "updatedBy")]
        public string UpdatedBy { get; set; }

        /// <value>
        /// A descriptive title for the Scrapbook101core item. For example, if item is a book, use all or part of the book title.
        /// </value>
        [Required(ErrorMessage = "Title is required.")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Defines the Document DB entry type. 
        /// </summary>
        /// <remarks>
        /// The value of the an item type is read from the appsettings.json file. For example, "scrapbook101Item". The
        /// type distinguished the item from other Document DB entries like category.
        /// </remarks>
        [Required(ErrorMessage = "Type is required.")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// Defines one asset in the <see cref="Scrapbook101core.Models.Item.Assets"/> property of 
    /// the <see cref="Scrapbook101core.Models.Item"/> class.
    /// </summary>
    public class AssetItem : IComparable<AssetItem>
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }

        public bool Equals(AssetItem other)
        {
            if (other == null) return false;
            return (this.Name.Equals(other.Name));
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            AssetItem objAsItem = obj as AssetItem;
            if (objAsItem == null) return false;
            else return Equals(objAsItem);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public int CompareTo(AssetItem other)
        {
            if (other == null)
                return 1;
            else
                return this.Name.CompareTo(other.Name);
        }
    }
}
