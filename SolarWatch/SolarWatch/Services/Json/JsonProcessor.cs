using SolarWatch.Models;
using SolarWatch.Models.SunriseSunset;
using System.Text.Json;

namespace SolarWatch.Services.Json
{
    public class JsonProcessor : IJsonProcessor
    {


        public GeocodingApiResponse GetGeocodingApiResponse(string data)
        {
            JsonDocument json = JsonDocument.Parse(data);
            JsonElement coord = json.RootElement.GetProperty("coord");

            Coordinates Coordinates = new Coordinates
            {
                Lat = coord.GetProperty("lat").GetDouble(),
                Lon = coord.GetProperty("lon").GetDouble()
            };

            GeocodingApiResponse response = new GeocodingApiResponse()
            {
                Coord = Coordinates
            };

            return response;
        }

        public SunriseSunsetResults Process(string data, string cityname, DateTime date)
        {
            JsonDocument json = JsonDocument.Parse(data);
            JsonElement res = json.RootElement.GetProperty("results");
            City city = new City { Name = cityname };
            SunriseSunsetResults latAndLon = new SunriseSunsetResults
            {
                City = city,
                Date = date,
                Sunrise = res.GetProperty("sunrise").GetDateTimeOffset().ToLocalTime(),
                Sunset = res.GetProperty("sunset").GetDateTimeOffset().ToLocalTime(),
            };

            return latAndLon;
        }

    }
}
