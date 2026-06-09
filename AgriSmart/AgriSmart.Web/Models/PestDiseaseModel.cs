namespace AgriSmart.Web.Models
{
    public class PestDiseaseItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // "Pest" or "Disease"
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Symptoms { get; set; }
        public string ControlMeasures { get; set; }
    }
}
