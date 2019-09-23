namespace Scrapbook101core
{
    using BingMapsRESTToolkit;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a static method to convert a string location to a geocode (latitude and longitude).
    /// </summary>
    public static class GeoCodeHelper
    {

        /// <summary>
        /// Gets the geocode (latitude, longitude) from a string location description.
        /// </summary>
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