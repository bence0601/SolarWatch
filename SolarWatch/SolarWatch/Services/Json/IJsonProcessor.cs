using SolarWatch.Models;
using SolarWatch.Models.SunriseSunset;

namespace SolarWatch.Services.Json
{
    public interface IJsonProcessor
    {
        GeocodingApiResponse GetGeocodingApiResponse(string data);

        SunriseSunsetResults Process(string data, string city, DateTime date);

    }
}
