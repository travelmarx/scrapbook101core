﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: api/ItemApi/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
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

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
