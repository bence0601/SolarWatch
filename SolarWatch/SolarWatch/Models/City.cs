using SolarWatch.Models.SunriseSunset;

namespace SolarWatch.Models
{
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Coordinates Coordinates { get; set; }
        public SunriseSunsetResults  SunriseSunsetInfo { get; set; }

    }
}
