namespace SolarWatch.Models.SunriseSunset
{
    public class SunriseSunsetResults
    {
public int ID { get; set; }
        public DateTime? Date { get; set; }
        public DateTimeOffset Sunrise { get; set; }
        public DateTimeOffset Sunset { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }

    }
}
