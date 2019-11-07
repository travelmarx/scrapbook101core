using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        /// Returns all the items in Scrapbook101.
        /// </summary>
        /// <remarks>
        /// Specify the HTTP GET method and the URI "baseURI/api/ItemApi".
        /// </remarks>
        /// <returns>JSON representing of all items.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Item>>> GetAsync()
        {
            try
            {
                var items = await DocumentDBRepository<Item>
                    .GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType);
                var imagePath = HelperClasses.BuildPathList(items);
                return Ok(items.ToList());
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
        /// </remarks>
        /// <param name="value">JSON representing the item.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async void PostAsync([FromBody] Item value)
        {
            Item newItem = new Item()
            {
                // Item properties that are auto-generated.
                DateAdded = System.DateTime.UtcNow,
                DateUpdated = System.DateTime.UtcNow,
                UpdatedBy = value.Location ?? "travelmarx",
                GeoLocation = null,
                Assets = null,
                Id = null   // Set to null so on insert CosmosDB sets the GUID.
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
                await DocumentDBRepository<Item>.CreateItemAsync(newItem);
                Accepted();
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
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
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
