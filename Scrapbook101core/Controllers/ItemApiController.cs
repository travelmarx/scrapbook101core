using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrapbook101core.Models;

namespace Scrapbook101core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemApiController : ControllerBase
    {

        // GET: api/ItemApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetAsync()
        {
            var items = await DocumentDBRepository<Item>
                .GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType);
            var imagePath = HelperClasses.BuildPathList(items);
            return items.ToList();
        }

        // GET: api/ItemApi/GUID
        [HttpGet("{id}", Name = "Details")]
        public async Task<ActionResult<Item>> DetailsAsync(string id)
        {
            var item = await DocumentDBRepository<Item>.GetItemAsync(id);
            return item;
        }

        // POST: api/ItemApi
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ItemApi/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/GUID
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
