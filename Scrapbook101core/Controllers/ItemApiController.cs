using System;
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
            var imagePath = BuildPathList(items);
            return items.ToList();
        }

        private List<string> BuildPathList(IEnumerable<Item> items)
        {
            List<string> imagePath = new List<string>();
            foreach (var item in items)
            {
                string imageToDisplay = AppVariables.NoAssetImage;
                if (!System.String.IsNullOrEmpty(item.AssetPath))
                {

                    if (item.Assets != null && item.Assets.Count != 0)
                    {
                        // Show first image found if one exists
                        foreach (var asset in item.Assets)
                        {
                            string contentType;
                            new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider().TryGetContentType(asset.Name, out contentType);
                            if (contentType.StartsWith("image/"))
                            {
                                imageToDisplay = $"{item.AssetPath}" + "/" + asset.Name;
                                break;
                            }
                        }
                    }
                    imagePath.Add(AppVariables.AssetBasePath + imageToDisplay);
                }
                else
                {
                    imagePath.Add(AppVariables.AssetBasePath + imageToDisplay);
                }
            }
            return imagePath;
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
