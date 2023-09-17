using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Models.SunriseSunset;
using SolarWatch.Services;
using SolarWatch.Services.Json;
using SolarWatch.Services.Repositories;


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
        private readonly ICityRepository _cityRepository;
        private readonly ISunriseSunsetRepository _sunriseSunsetRepository;

        public SolarController(ILogger<SolarController> logger, IWeatherDataProvider weatherDataProvider,
            IJsonProcessor jsonProcessor, ISunriseSunsetRepository sunriseSunsetRepository, ICityRepository cityRepository)
        {
            _logger = logger;
            _weatherDataProvider = weatherDataProvider;
            _jsonProcessor = jsonProcessor;
            _httpClient = new HttpClient();
            _sunriseSunsetRepository = sunriseSunsetRepository;
            _cityRepository = cityRepository;
        }





        [HttpGet]
        [Route("api/solar"), Authorize]
        public async Task<ActionResult<SunriseSunsetResults>> GetSunriseSunset(string cityname, DateTime date)
        {
            var city = _cityRepository.GetByName(cityname);
            if (city == null)
            {
                var geoData = await  _weatherDataProvider.GetLatLonAsync(cityname);
                var geoResult = _jsonProcessor.GetGeocodingApiResponse(geoData);

                var lat = geoResult.Coord.Lat;
                var lon = geoResult.Coord.Lon;
                city = new City { Name = cityname, Coordinates = new Coordinates { Lat = lat, Lon = lon } };
                await _cityRepository.AddAsync(city);
            }
            try
            {
                var sunriseSunset = await _sunriseSunsetRepository.GetByCityAndDateAsync(city.ID, date.Date);
                if (sunriseSunset == null)
                {
                    var weatherData =await _weatherDataProvider.GetSunriseSunsetAsync(city.Coordinates.Lat, city.Coordinates.Lon, date);
                    var sunriseSunsetData = _jsonProcessor.Process(weatherData, cityname, date);
                    sunriseSunsetData.City = city;
                    await _sunriseSunsetRepository.AddAsync(new SunriseSunsetResults{CityId = city.ID, Sunrise = sunriseSunsetData.Sunrise, Sunset = sunriseSunsetData.Sunset,Date = date});
                    return Ok(sunriseSunsetData);
                    
                }

                return Ok(sunriseSunset);


            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting weather data");
                return NotFound("Error getting weather data");
            }
        }

    }
}
