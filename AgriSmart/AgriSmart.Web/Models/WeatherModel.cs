using System.Collections.Generic;

namespace AgriSmart.Web.Models
{
    public class WeatherData
    {
        public string City { get; set; }
        public string Province { get; set; }
        public int Temperature { get; set; }
        public string Condition { get; set; }
        public int Humidity { get; set; }
        public int WindSpeed { get; set; }
        public int Pressure { get; set; }
        public List<HourlyForecast> HourlyForecasts { get; set; } = new List<HourlyForecast>();
        public List<DailyForecast> DailyForecasts { get; set; } = new List<DailyForecast>();
    }

    public class HourlyForecast
    {
        public string Time { get; set; }
        public int Temperature { get; set; }
        public string IconClass { get; set; } // e.g., "oi-sun", "oi-cloud", "oi-rain"
    }

    public class DailyForecast
    {
        public string Day { get; set; } // e.g. "Wed, 21 May"
        public int MaxTemp { get; set; }
        public int MinTemp { get; set; }
        public string IconClass { get; set; }
    }
}
