using System.Collections.Generic;

namespace AgriSmart.Web.Helpers
{
    public static class PakistanData
    {
        public static readonly IReadOnlyList<string> Provinces = new[]
        {
            "Punjab", "Sindh", "KPK", "Balochistan", "ICT", "AJK", "GB"
        };

        public static readonly IReadOnlyList<string> Seasons = new[] { "Kharif", "Rabi", "Year-Round" };

        public static readonly IReadOnlyList<string> SoilTypes = new[] { "Loamy", "Clay", "Sandy", "Silt" };

        public static readonly IReadOnlyList<string> Cities = new[]
        {
            "Karachi", "Lahore", "Islamabad", "Peshawar", "Quetta", "Faisalabad", "Multan",
            "Rawalpindi", "Hyderabad", "Gujranwala", "Sialkot", "Bahawalpur", "Sargodha",
            "Abbottabad", "Mardan", "Sukkur", "Larkana", "Mirpur Khas", "Dera Ghazi Khan",
            "Sheikhupura", "Jhang", "Gujrat", "Sahiwal", "Wah Cantonment", "Mingora",
            "Nawabshah", "Khuzdar", "Turbat", "Dera Ismail Khan", "Kohat", "Muzaffarabad",
            "Mirpur AJK", "Gilgit", "Skardu", "Jacobabad", "Kasur"
        };
    }
}
