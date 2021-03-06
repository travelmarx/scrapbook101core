﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scrapbook101core.Models;

namespace Scrapbook101core.Controllers
{
    /// <summary>
    /// In the Model-View-Controller pattern, the controller processes the input. In this case, it is all the input involving
    /// item operations. The most important method is the
    /// <see cref="Scrapbook101core.Controllers.ItemController.IndexAsync"/>, which
    /// returns a list of items.
    /// </summary>
    public class ItemController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ItemController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Returns items based that contain the specified string. Note that for demonstration, the functionality
        /// is purposely limited. The string could also be found in description field. Similarly, more parameters
        /// can be added to expand the possibility of the searches.
        /// </summary>
        /// <param name="filter">A string to find in titles.</param>
        /// <returns>A list of matching items</returns>
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync(string filter)
        {
            if (String.IsNullOrEmpty(filter))
            {
                var items = await DocumentDBRepository<Item>.GetItemsAsync(
                    item => item.Type == AppVariables.ItemDocumentType,
                    item => item.DateAdded);
                ViewBag.imagePath = HelperClasses.BuildPathList(items);
                return View(items);
            }
            else
            {
                var items = await DocumentDBRepository<Item>.GetItemsAsync(
                    item => item.Type == AppVariables.ItemDocumentType
                    && (item.Title.ToLower().Contains(filter.ToLower()))
                    || (item.Description.ToLower().Contains(filter.ToLower())),
                    item => item.DateAdded);
                ViewBag.imagePath = HelperClasses.BuildPathList(items);
                return View(items);
            }
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
            combinedModel.Item.Type = AppVariables.ItemDocumentType;

            // Get geocode from Bing if applicable - see web.config
            if (AppVariables.BingMapKey.Length > 0)
            {
                double[] coord = await HelperClasses.GetGeocode(combinedModel.Item.Location);
                if (coord[0] != 0)
                {
                    combinedModel.Item.GeoLocation = new Microsoft.Azure.Documents.Spatial.Point(coord[1], coord[0]);
                }
            }

            // Check for local upoads - the LocalHttpPostedFileBaseList object contains the streams
            if (combinedModel.LocalUploadFileList?[0] != null && combinedModel.LocalUploadFileList.Count() > 0)
            {
                foreach (var file in combinedModel.LocalUploadFileList)
                {
                    if (combinedModel.Item.Assets == null || combinedModel.Item.Assets.Count() == 0)
                    {
                        combinedModel.Item.Assets = new List<AssetItem>();
                    }

                    combinedModel.Item.Assets.Add(
                        new AssetItem
                        {
                            Name = file.Name,
                            Size = file.Size
                        });
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
            bool existFilesToRemove = HttpContext.Request.Form.TryGetValue("FilesToRemove", out var filesToRemove);

            if (existFilesToRemove)
            {
                foreach (var file in filesToRemove)
                {
                    combinedModel.Item.Assets.Remove(new AssetItem() { Name = file, Size = "n/a" });
                }
            }
            // Check for local upoads - the LocalHttpPostedFileBaseList object contains the streams
            if (combinedModel.LocalUploadFileList?[0] != null && combinedModel.LocalUploadFileList.Count() > 0)
            {
                foreach (var file in combinedModel.LocalUploadFileList)
                {
                    if (combinedModel.Item.Assets == null || combinedModel.Item.Assets.Count() == 0)
                    {
                        combinedModel.Item.Assets = new List<AssetItem>();
                    }

                    combinedModel.Item.Assets.Add(
                        new AssetItem
                        {
                            Name = file.Name,
                            Size = file.Size
                        });
                }
                // Add sorted items
                combinedModel.Item.Assets = combinedModel.Item.Assets.OrderBy(a => a.Name).ToList();
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