namespace Scrapbook101core
{
    using BingMapsRESTToolkit;
    using System;
    using System.Threading.Tasks;

    public static class GeoCodeHelper
    {
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