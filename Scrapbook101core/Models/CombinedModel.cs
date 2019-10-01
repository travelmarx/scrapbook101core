namespace Scrapbook101core.Models
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a MVC convenience model for display purposes in some view pages.
    /// </summary>
    /// <remarks>
    /// This model combine the <see cref="Scrapbook101core.Models.Item"/> class/model with 
    /// lists of categories and lists of files that are useful for pages that require both
    /// the Item class and category and file support.
    /// </remarks>
    public class CombinedModel
    {
        public Item Item { get; set; }

        public List<CategoryItemDisplay> CategoryItemsForDisplay { get; set; }
        public List<CategoryFieldMapping> CategoryFieldMappingList { get; set; }
        public List<FileItem> LocalUploadFileList { get; set; }
        public List<IFormFile> LocalHttpPostedFileList { get; set; }
    }

    /// <summary>
    /// Represents one category for display purposes.
    /// </summary>
    public class CategoryItemDisplay
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CategoryInfo { get; set; }
    }

    /// <summary>
    /// Used for tracking what fields are valid for a category. Not used to deserialize JSON.
    /// </summary>
    public class CategoryFieldMapping
    {
        public string Name { get; set; }
        public List<string> ActiveFields { get; set; }
    }

    /// <summary>
    /// Represents a file asset that uploaded locally.
    /// </summary>
    public class FileItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Role { get; set; }
        public string Size { get; set; }
    }
}
