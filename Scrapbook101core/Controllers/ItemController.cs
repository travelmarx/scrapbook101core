﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Scrapbook101core.Models;

namespace Scrapbook101core.Controllers
{
    public class ItemController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ItemController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var items = await DocumentDBRepository<Item>.GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType);
            ViewBag.imagePath = BuildPathList(items);
            return View(items);
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
                            new FileExtensionContentTypeProvider().TryGetContentType(asset.Name, out contentType);
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

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            Item item = await DocumentDBRepository<Item>.GetItemAsync(id);
            CombinedModel combinedModel = new CombinedModel
            {
                Item = item,
                CategoryItemsForDisplay = AppVariables.CategoryDisplayList,
                CategoryFieldMappingList = AppVariables.CategoryFieldMappingList
            };
            return View(combinedModel);
        }

#pragma warning disable 1998
        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            CombinedModel combinedModel = new CombinedModel
            {
                Item = new Item
                {
                    DateAdded = System.DateTime.UtcNow,
                    DateUpdated = System.DateTime.UtcNow,
                    Type = AppVariables.ItemDocumentType
                },
                CategoryItemsForDisplay = AppVariables.CategoryDisplayList
            };
            return View(combinedModel);
        }
#pragma warning restore 1998

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CombinedModel combinedModel)
        {
            // The items not bound are specifed
            combinedModel.Item.DateAdded = System.DateTime.UtcNow;
            combinedModel.Item.DateUpdated = System.DateTime.UtcNow;
            combinedModel.Item.UpdatedBy = "travelmarx"; // If user is signed in, use name or email.
            combinedModel.Item.GeoLocation = null;
            combinedModel.Item.Assets = null;
            combinedModel.Item.Id = null; // Set to null so on insert CosmosDB sets GUID.

            // Get geocode from Bing if applicable - see web.config
            if (AppVariables.BingMapKey.Length > 0)
            {
                double[] coord = await GeoCodeHelper.GetGeocode(combinedModel.Item.Location);
                if (coord[0] != 0)
                {
                    combinedModel.Item.GeoLocation = new Microsoft.Azure.Documents.Spatial.Point(coord[1], coord[0]);
                }
            }

            // Check for local upoads - the LocalHttpPostedFileBaseList object contains the streams
            if (combinedModel.LocalHttpPostedFileList?[0] != null && combinedModel.LocalHttpPostedFileList.Count() > 0)
            {
                int indx = 0;
                foreach (var file in combinedModel.LocalHttpPostedFileList)
                {
                    FileItem currFile = combinedModel.LocalUploadFileList[indx];

                    if (combinedModel.Item.Assets == null || combinedModel.Item.Assets.Count() == 0)
                    {
                        combinedModel.Item.Assets = new List<AssetItem>();
                    }

                    combinedModel.Item.Assets.Add(
                        new AssetItem
                        {
                            Name = currFile.Name,
                            Size = currFile.Size
                        });
                    indx++;
                }
                // Add sorted items
                combinedModel.Item.Assets = combinedModel.Item.Assets.OrderBy(a => a.Name).ToList();
            }

            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.CreateItemAsync(combinedModel.Item);
                return RedirectToAction("Index");
            }

            return View(combinedModel);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return StatusCode(400);
            }

            Item item = await DocumentDBRepository<Item>.GetItemAsync(id);
            if (item == null)
            {
                return StatusCode(404);
            }

            CombinedModel combinedModel = new CombinedModel
            {
                Item = item,
                CategoryItemsForDisplay = AppVariables.CategoryDisplayList,
                CategoryFieldMappingList = AppVariables.CategoryFieldMappingList
            };

            return View(combinedModel);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(CombinedModel combinedModel)
        {
            // Deal with any files checked for removal.
            bool existFilesToRemove = HttpContext.Request.Headers.TryGetValue("FilesToRemove", out var filesToRemove);

            if (existFilesToRemove)
            {
                foreach (var file in filesToRemove)
                {
                    combinedModel.Item.Assets.Remove(new AssetItem() { Name = file, Size = "n/a" });
                }
            }
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.UpdateItemAsync(combinedModel.Item.Id, combinedModel.Item);
                return RedirectToAction("Index");
            }

            return View(combinedModel);
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return StatusCode(400);
            }

            Item item = await DocumentDBRepository<Item>.GetItemAsync(id);
            if (item == null)
            {
                return StatusCode(404);
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] string id)
        {
            await DocumentDBRepository<Item>.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }
    }
}