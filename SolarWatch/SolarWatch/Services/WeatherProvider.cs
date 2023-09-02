using System.Net;

namespace SolarWatch.Services
{
    public class WeatherProvider : IWeatherDataProvider
    {
        private readonly ILogger<WeatherProvider> _logger;
        private readonly string apiKey = "410b77bb7ea5e4483af51a593d71c09d";
        public WeatherProvider(ILogger<WeatherProvider> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetLatLonAsync(string city)
        {
            var geoUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(geoUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        return content;
                    }
                    else
                    {
                        _logger.LogError("Failed to retrieve data from OpenWeather API. Status code: {statusCode}", response.StatusCode);
                        // Handle non-successful response here
                        return null; // or throw an exception if needed
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error while making the HTTP request to OpenWeather API");
                // Handle the exception
                return null; // or throw an exception if needed
            }
        }


        public async Task<string> GetSunriseSunsetAsync(double lat, double lon, DateTime date)
        {
            var sunriseSunsetUrl = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date:yyyy-MM-dd}&formatted=0";
            var client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(sunriseSunsetUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    _logger.LogError("Failed to retrieve data from OpenWeather API. Status code: {statusCode}", response.StatusCode);
                    
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error while making the HTTP request to OpenWeather API");
               
                return null; 
            }
            finally
            {
                
                client.Dispose();
            }
        }
    }
}
