using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch.Services.Repositories
{
    public class CityRepository : ICityRepository
    {
        public async Task<City> GetCityByIdAsync(int cityId)
        {
            using var dbContext = new SolarWatchApiContext();
            return await dbContext.Cities
                .Include(c => c.SunriseSunsetInfo)
                .SingleOrDefaultAsync(c => c.ID == cityId);
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            using var dbContext = new SolarWatchApiContext();
            return await dbContext.Cities.ToListAsync();
        }

        public City GetByName(string name)
        {
            using var dbContext = new SolarWatchApiContext();
            return dbContext.Cities.FirstOrDefault(c => c.Name == name);
        }

        public async Task AddAsync(City city)
        {
            using var dbContext = new SolarWatchApiContext();
            dbContext.Add(city);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(City city)
        {
            using var dbContext = new SolarWatchApiContext();
            dbContext.Remove(city);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(City city)
        {
            using var dbContext = new SolarWatchApiContext();
            dbContext.Update(city);
            await dbContext.SaveChangesAsync();
        }
    }
}
