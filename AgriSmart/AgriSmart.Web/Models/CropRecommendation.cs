namespace AgriSmart.Web.Models
{
    public class CropRecommendation
    {
        public string Name { get; set; }

        /// <summary>High, Medium, or Low</summary>
        public string Suitability { get; set; }

        /// <summary>Human-readable duration, e.g. "120 - 140 days"</summary>
        public string Duration { get; set; }

        /// <summary>Open Iconic icon class, e.g. "oi-sun"</summary>
        public string IconClass { get; set; }
    }
}
