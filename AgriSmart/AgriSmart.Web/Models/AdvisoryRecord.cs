using System;

namespace AgriSmart.Web.Models
{
    public class AdvisoryRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CropName { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; } // Fertilizer, Pest Control, Irrigation, Harvesting
    }
}
