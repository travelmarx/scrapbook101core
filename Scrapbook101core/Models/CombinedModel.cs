namespace Scrapbook101core.Models
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public class CombinedModel
    {
        public Item Item { get; set; }

        public List<CategoryItemDisplay> CategoryItemsForDisplay { get; set; }
        public List<CategoryFieldMapping> CategoryFieldMappingList { get; set; }
        public List<FileItem> LocalUploadFileList { get; set; }
        public List<IFormFile> LocalHttpPostedFileList { get; set; }
    }

    public class CategoryItemDisplay
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CategoryInfo { get; set; }
    }

    // Not used to deserialize JSON; for tracking what fields are valid for a category
    public class CategoryFieldMapping
    {
        public string Name { get; set; }
        public List<string> ActiveFields { get; set; }
    }

    public class FileItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Role { get; set; }
        public string Size { get; set; }
    }
}
