using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models.SunriseSunset;
using SolarWatch.Services;
using SolarWatch.Services.Json;


namespace SolarWatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolarController : Controller
    {
        private readonly ILogger<SolarController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IWeatherDataProvider _weatherDataProvider;
        private readonly IJsonProcessor _jsonProcessor;

        public SolarController(ILogger<SolarController> logger, IWeatherDataProvider weatherDataProvider,
            IJsonProcessor jsonProcessor)
        {
            _logger = logger;
            _weatherDataProvider = weatherDataProvider;
            _jsonProcessor = jsonProcessor;
            _httpClient = new HttpClient();
        }





        [HttpGet]
        [Route("api/solar")]
        public async Task<ActionResult<SunriseSunsetResults>> GetSunriseSunset(string city, DateTime date)
        {
            try
            {
                var geoDataTask = _weatherDataProvider.GetLatLonAsync(city);
                var geoData = await geoDataTask; // Await the asynchronous task to get the result
                var geoResult = _jsonProcessor.GetGeocodingApiResponse(geoData);

                var lat = geoResult.Coord.Lat;
                var lon = geoResult.Coord.Lon;

                var weatherDataTask = _weatherDataProvider.GetSunriseSunsetAsync(lat, lon, date);
                var weatherData = await weatherDataTask; // Await the asynchronous task to get the result

                return Ok(_jsonProcessor.Process(weatherData, city, date));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting weather data");
                return NotFound("Error getting weather data");
            }
        }

    }
}
