using SolarWatch.Models;

namespace SolarWatch.Services.Repositories
{
    public interface ICityRepository
    {
        Task<City> GetCityByIdAsync(int cityId);
        Task<IEnumerable<City>> GetAllCitiesAsync();
        City GetByName(string name);
        Task AddAsync(City city);
        Task DeleteAsync(City city);
        Task UpdateAsync(City city);
    }
}
