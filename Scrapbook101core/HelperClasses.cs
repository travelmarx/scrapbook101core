namespace Scrapbook101core
{
    using BingMapsRESTToolkit;
    using Microsoft.AspNetCore.StaticFiles;
    using Scrapbook101core.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines helper methods for use in Scrapbook101core.
    /// </summary>
    public static class HelperClasses
    {
        /// <summary>
        /// Returns paths for images to display for a list of Scrapbook101core items. 
        /// </summary>
        /// <remarks>
        /// For each Scrapbook101core item, one image is found or the default image is used.
        /// </remarks>
        /// <param name="items">A list of Scrapbook101core items to get the qualified path.</param>
        /// <returns></returns>
        public static System.Collections.Generic.List<string> BuildPathList(IEnumerable<Item> items)
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


        /// <summary>
        /// A static method to convert a string location to a geocode (latitude and longitude).
        /// </summary>
        /// <remarks>
        /// Gets the geocode (latitude, longitude) from a string location description.
        /// </remarks>
        /// <param name="location">A string describing a location. For example, "Seattle, WA" or "Italy".</param>
        /// <returns>A struct of doubles representing longitude and latitude.</returns>
        public static async Task<double[]> GetGeocode(string location)
        {
            var noCoord = new double[] { 0, 0 };

            // Create a request
            if (!String.IsNullOrEmpty(location))
            {
                var request = new GeocodeRequest()
                {
                    Query = location,
                    IncludeIso2 = false,
                    IncludeNeighborhood = false,
                    MaxResults = 1,
                    BingMapsKey = AppVariables.BingMapKey
                };

                //Process the request by using the ServiceManager.
                var response = await request.Execute();

                if (response != null &&
                    response.StatusCode == 200 &&
                    response.ResourceSets != null &&
                    response.ResourceSets.Length > 0 &&
                    response.ResourceSets[0].Resources != null &&
                    response.ResourceSets[0].Resources.Length > 0)
                {
                    var result = response.ResourceSets[0].Resources[0] as BingMapsRESTToolkit.Location;
                    return result.Point.Coordinates;  // latitude, longitude
                }
                else
                {
                    return noCoord;
                }
            }
            else
            {
                return noCoord;
            }
        }
    }
}