using SolarWatch.Models.SunriseSunset;

namespace SolarWatch.Services.Repositories
{
    public interface ISunriseSunsetRepository
    {
        Task<SunriseSunsetResults> GetByCityAndDateAsync(int cityId, DateTime date);
        Task AddAsync(SunriseSunsetResults sunriseSunset);
    }
}
