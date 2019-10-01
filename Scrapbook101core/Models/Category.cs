namespace Scrapbook101core.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the categories in Scrapbook101core.
    /// </summary>
    /// <remarks>
    /// The list of categories is read in from one document from the database identified by
    /// an <see cref="Scrapbook101core.Models.Category.Id"/> and <see cref="Scrapbook101core.Models.Category.Type"/>.
    /// </remarks>
    public class Category
    {
        [Required(ErrorMessage = "id is required.")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "categories")]
        public List<CategoryItem> Categories { get; set; }
    }

    /// <summary>
    /// Defines one category inside the <see cref="Scrapbook101core.Models.Category.Categories"/> list of the
    /// <see cref="Scrapbook101core.Models.Category"/> class.
    /// </summary>
    public class CategoryItem
    {
        [Required(ErrorMessage = "Name is required.")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "categoryFields")]
        public CategoryFields CategoryFields { get; set; }
    }

    /// <summary>
    /// Represents all the possible category fields. A category field may apply to some categories and not to others.
    /// </summary>
    public class CategoryFields
    {

        // Allows accessing properties by string name as index
        // JsonIgnore attribute is key for deserialization and initialization checks for category
        [JsonIgnore]
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value); }
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "artist")]
        public string Artist { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "author")]
        public string Author { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "birthDate")]
        public string BirthDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "director")]
        public string Director { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "genre")]
        public string Genre { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "highlight")]
        public string Highlight { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "lodging")]
        public string Lodging { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "synopsis")]
        public string Synopsis { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "theme")]
        public string Theme { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "who")]
        public string Who { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Year")]
        public string Year { get; set; }
    }
}
