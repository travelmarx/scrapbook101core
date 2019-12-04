using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Scrapbook101core.Models;

namespace Scrapbook101core.Controllers
{
    /// <summary>
    /// Defines the Web API controller that handles HTTP methods GET, PUT, POST, and DELETE.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ItemApiController : ControllerBase
    {

        /// <summary>
        /// Returns Scrapbook101 items whose title or body matches filter. If filter is omitted, all items are returned.
        /// </summary>
        /// <remarks>
        /// Specify the HTTP GET method and the URI "baseURI/api/ItemApi?filter=filter".
        /// </remarks>
        /// <param name="filter">Optional string to find in titles and descriptions.</param>
        /// <returns>JSON representing of all matching items.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Item>>> GetAsync([FromQuery] string filter)
        {
            try
            {
                if (String.IsNullOrEmpty(filter))
                {
                    var items = await DocumentDBRepository<Item>
                        .GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType);
                    var imagePath = HelperClasses.BuildPathList(items);
                    return Ok(items.ToList());
                }
                else
                {
                    var items = await DocumentDBRepository<Item>
                        .GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType
                            && (item.Title.ToLower().Contains(filter.ToLower()))
                            || (item.Description.ToLower().Contains(filter.ToLower())));
                    var imagePath = HelperClasses.BuildPathList(items);
                    return Ok(items.ToList());
                }
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: api/ItemApi/GUID
        /// <summary>
        /// Return the item matching the specified GUID.
        /// </summary>
        /// <remarks>
        /// Specify the HTTP GET method and the URI "baseURI/api/ItemApi/GUID".
        /// </remarks>
        /// <param name="id">The GUID of the item to return.</param>
        /// <returns>JSON representing the specified item.</returns>
        [HttpGet("{id}", Name = "Details")]
        public async Task<ActionResult<Item>> DetailsAsync(string id)
        {
            var item = await DocumentDBRepository<Item>.GetItemAsync(id);
            return item;
        }

        /// <summary>
        /// Creates a new Scrapbook101core item.
        /// </summary>
        /// <remarks>
        /// Specify the HTTP POST method, the URI "baseURI/api/ItemApi", and the item details in the request body.
        /// 
        /// Required fields are determined by the Required attribute used in the <see cref="Scrapbook101core.Models.Item"/> class definition. 
        /// Not specifying a required item results in an 400 response.
        /// At least the Title, Type, and Category of the item must be specified.
        /// Do not specify the item ID as it will be auto-assigned when saved.
        /// </remarks>
        /// <param name="value">JSON representing the item.</param>
        [HttpPost(Name = "Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> CreateAsync([FromBody] Item value)
        {
            Item newItem = new Item()
            {
                Id = null,   // Set to null so on insert CosmosDB sets the GUID.
                Type = value.Type,
                Category = value.Category,
                Title = value.Title,
                Location = value.Location ?? "",
                DateAdded = System.DateTime.UtcNow,
                DateUpdated = System.DateTime.UtcNow,
                UpdatedBy = value.UpdatedBy ?? "travelmarx",
                GeoLocation = null,
                Description = value.Description,
                AssetPath = value.AssetPath ?? null,
                Assets = value.Assets ?? null,
                CategoryFields = value.CategoryFields ?? null,
                Rating = value.Rating ?? null
            };

            // Get geocode from Bing if applicable - see web.config
            if (AppVariables.BingMapKey.Length > 0)
            {
                double[] coord = await HelperClasses.GetGeocode(value.Location);
                if (coord[0] != 0)
                {
                    newItem.GeoLocation = new Microsoft.Azure.Documents.Spatial.Point(coord[1], coord[0]);
                }
            }

            if (ModelState.IsValid)
            {
                Document created = await DocumentDBRepository<Item>.CreateItemAsync(newItem);
                return Ok(created.Id);
            }
            else
            {
                return BadRequest("Item not created.");
            }
        }

        /// <summary>
        /// Updates an existing Scrapbook101core item.
        /// </summary>
        /// <remarks>
        /// Specify the HTTP PUT method, the URI "baseURI/api/ItemApi/GUID", and the items details in the request body.
        /// </remarks>
        /// <param name="id">The GUID of the item to update.</param>
        /// <param name="value">JSON representing the item to update.</param>
        [HttpPut("{id}", Name = "Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> UpdateAsync(string id, [FromBody] Item value)
        {
            var originalItem = await DocumentDBRepository<Item>.GetItemAsync(id);

            Item updatedItem = new Item()
            {
                Id = id,
                Type = originalItem.Type,
                Category = value.Category,
                Title = value.Title ?? originalItem.Title,
                Location = value.Location ?? originalItem.Location,
                DateAdded = originalItem.DateAdded,
                DateUpdated = System.DateTime.UtcNow,
                UpdatedBy = value.UpdatedBy ?? originalItem.UpdatedBy,
                GeoLocation = null,
                Description = value.Description ?? originalItem.Description,
                AssetPath = value.AssetPath ?? originalItem.AssetPath,
                Assets = value.Assets ?? originalItem.Assets,
                CategoryFields = value.CategoryFields ?? originalItem.CategoryFields,
                Rating = value.Rating ?? originalItem.Rating
            };

            // Get geocode from Bing if applicable - see web.config
            if (AppVariables.BingMapKey.Length > 0)
            {
                double[] coord = await HelperClasses.GetGeocode(value.Location);
                if (coord[0] != 0)
                {
                    updatedItem.GeoLocation = new Microsoft.Azure.Documents.Spatial.Point(coord[1], coord[0]);
                }
            }

            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.UpdateItemAsync(id, updatedItem);
                return Ok("Item updated.");
            }
            else
            {
                return BadRequest("Item not updated.");
            }
        }

        /// <summary>
        /// Deletes the item matching the specified GUID.
        /// </summary>
        /// <remarks>
        /// Specify the HTTP method DELETE and the URI "baseURI/api/ItemApi/GUID".
        /// </remarks>
        /// <param name="id">The GUID of the item to delete.</param>
        [HttpDelete("{id}", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async void DeleteAsync(string id)
        {
            if (id == null)
            {
                BadRequest();
            }

            Item item = await DocumentDBRepository<Item>.GetItemAsync(id);
            if (item == null)
            {
                NotFound();
            }
            else
            {
                await DocumentDBRepository<Item>.DeleteItemAsync(id);
                Accepted();
            }
        }
    }
}
