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
        /// A list of assets associated with the Item. There can be no assets associated.
        /// </value>
        [JsonProperty(PropertyName = "assets")]
        public List<AssetItem> Assets { get; set; }

        [JsonProperty(PropertyName = "assetPath")]
        public string AssetPath { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "categoryFields")]
        public CategoryFields CategoryFields { get; set; }

        [JsonProperty(PropertyName = "dateAdded")]
        public DateTime DateAdded { get; set; }

        [JsonProperty(PropertyName = "dateUpdated")]
        public DateTime DateUpdated { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "geoLocation")]
        public Point GeoLocation { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public string Rating { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public string UpdatedBy { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

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
