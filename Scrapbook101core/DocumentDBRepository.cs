using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using Scrapbook101core.Models;

namespace Scrapbook101core
{
    /// <summary>
    /// Defines static variables that map to known values at application start and that 
    /// don't change during the application life time. These static variables mostly
    /// map to the configu values in the <c>appsettings.json</c> file. For example,
    /// the Document DB database name.
    /// </summary>
    public static class AppVariables
    {
        public static readonly bool AddTestAssets = Scrapbook101Configuration.Current.AddTestAssets;
        public static readonly string ApplicationName = Scrapbook101Configuration.Current.ApplicationName;
        public static readonly string AssetBasePath = Scrapbook101Configuration.Current.AssetBasePath;
        public static readonly string AuthKey = Scrapbook101Configuration.Current.AuthKey;
        public static readonly string NoAssetImage = Scrapbook101Configuration.Current.NoAssetImage;
        public static readonly string BingMapKey = Scrapbook101Configuration.Current.BingMapKey;
        public static readonly string CategoryDocumentType = Scrapbook101Configuration.Current.CategoryDocumentType;
        public static readonly string CollectionId = Scrapbook101Configuration.Current.CollectionId;
        public static readonly string DatabaseId = Scrapbook101Configuration.Current.DatabaseId;
        public static readonly string Endpoint = Scrapbook101Configuration.Current.Endpoint;
        public static readonly string ItemDocumentType = Scrapbook101Configuration.Current.ItemDocumentType;
        public static readonly string TestCategories = Scrapbook101Configuration.Current.TestCategories;
        public static readonly string TestItems = Scrapbook101Configuration.Current.TestItems;

        public static List<CategoryItemDisplay> CategoryDisplayList { get; set; }
        public static List<CategoryFieldMapping> CategoryFieldMappingList { get; set; }
    }

    /// <summary>
    /// Defines the methods for working with the Document DB repository. Also contains methods for initializing
    /// a new repository if the application is configured to do so.
    /// </summary>
    /// <typeparam name="T">The Item class which represents a repository item.</typeparam>
    public static class DocumentDBRepository<T> where T : class
    {
        private static readonly string DatabaseId = AppVariables.DatabaseId;
        private static readonly string CollectionId = AppVariables.CollectionId;
        private static readonly bool InsertTestAssets = AppVariables.AddTestAssets;
        private static DocumentClient client;

        public static async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<Document> CreateItemAsync(T item)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        public static async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

        public static async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(AppVariables.Endpoint), AppVariables.AuthKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
            CreateCategoryDocumentIfNotExistsAsync().Wait();
            if (InsertTestAssets)
            {
                InsertTestItemDocumentsAsync().Wait();
            }
            if (AppVariables.CategoryDisplayList == null)
            {
                GetCategoryListDocumentAsync();
            }
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCategoryDocumentIfNotExistsAsync()
        {
            try
            {
                // The GUID specified must match what is in /Assets/categories-document.json
                var docUri = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, "49916d87-e565-4220-8806-b9e60c867ae6");
                await client.ReadDocumentAsync(docUri);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Insert category record
                    JObject categoryDoc = JObject.Parse(File.ReadAllText(AppVariables.TestCategories));
                    await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), categoryDoc, null, true);
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task InsertTestItemDocumentsAsync()
        {
            JObject books = JObject.Parse(File.ReadAllText(AppVariables.TestItems));
            dynamic results = Newtonsoft.Json.JsonConvert.DeserializeObject(books.ToString());
            foreach (var result in results.testItems)
            {
                var oneItem = Newtonsoft.Json.JsonConvert.DeserializeObject(result.ToString());
                var id = oneItem["id"].Value;
                if (oneItem["dateAdded"].Value == "autogenerated")
                {
                    oneItem["dateAdded"].Value = DateTime.UtcNow.ToString();
                }
                if (oneItem["dateUpdated"].Value == "autogenerated")
                {
                    oneItem["dateUpdated"].Value = DateTime.UtcNow.ToString();
                }
                try
                {
                    var docUri = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id);
                    await client.ReadDocumentAsync(docUri);
                }
                catch (DocumentClientException e)
                {
                    if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), oneItem, null, true);
                    }
                    else
                    { throw; }

                }
            }
        }

        public static async Task<bool> GetCategoryListDocumentAsync()
        {
            List<CategoryItemDisplay> categoryDisplayList = new List<CategoryItemDisplay>();
            List<CategoryFieldMapping> categoryFieldMappingList = new List<CategoryFieldMapping>();

            try
            {
                IDocumentQuery<Category> query = client.CreateDocumentQuery<Category>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId))
                    .Select(item => item)
                    .Where(item => item.Type == Scrapbook101Configuration.Current.CategoryDocumentType)
                    .AsDocumentQuery();

                List<Category> results = new List<Category>();
                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<Category>());
                }

                foreach (var category in results[0].Categories)
                {
                    // Categories object for dropdown lists
                    categoryDisplayList.Add(new CategoryItemDisplay { Name = category.Name, Id = category.Name });

                    // Categories object to keep track of only applicable object fields
                    CategoryFieldMapping mapping = new CategoryFieldMapping
                    {
                        Name = category.Name,
                        ActiveFields = new List<string>()
                    };

                    IList<System.Reflection.PropertyInfo> props = category.CategoryFields.GetType().GetProperties().ToList();
                    foreach (var prop in props)
                    {
                        if (prop.GetCustomAttributesData()[0].AttributeType.Name != "JsonIgnoreAttribute") // allow all to pass except indexer which is ignored
                        {
                            if (prop.GetValue(category.CategoryFields) != null)
                            {
                                mapping.ActiveFields.Add(prop.Name);
                            }
                        }
                    }
                    categoryFieldMappingList.Add(mapping);
                }
                AppVariables.CategoryDisplayList = categoryDisplayList;
                AppVariables.CategoryFieldMappingList = categoryFieldMappingList;
                return true;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
