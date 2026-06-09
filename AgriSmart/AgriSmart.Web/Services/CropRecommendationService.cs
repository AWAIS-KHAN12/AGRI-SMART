using System;
using System.Collections.Generic;
using System.Linq;
using AgriSmart.Web.Models;

namespace AgriSmart.Web.Services
{
    public class CropRecommendationService
    {
        private static readonly List<CropEntry> _crops = new List<CropEntry>
        {
            // Kharif crops
            new CropEntry("Rice",       "Kharif", new[]{"Punjab","Sindh","KPK"},                  new[]{"Loamy","Clay","Silt"},          "High",   "120 - 140 days", "oi-beaker"),
            new CropEntry("Maize",      "Kharif", new[]{"Punjab","KPK","AJK"},                    new[]{"Loamy","Sandy","Silt"},          "High",   "90 - 100 days",  "oi-sun"),
            new CropEntry("Cotton",     "Kharif", new[]{"Punjab","Sindh"},                         new[]{"Loamy","Clay"},                  "Medium", "150 - 180 days", "oi-cloud"),
            new CropEntry("Soybean",    "Kharif", new[]{"Punjab","KPK"},                           new[]{"Loamy","Clay","Sandy"},          "Medium", "100 - 120 days", "oi-droplet"),
            new CropEntry("Sugarcane",  "Kharif", new[]{"Punjab","Sindh","KPK"},                  new[]{"Loamy","Clay"},                  "High",   "270 - 365 days", "oi-bolt"),
            new CropEntry("Millet",     "Kharif", new[]{"Punjab","Sindh","Balochistan"},           new[]{"Sandy","Loamy"},                 "Medium", "60 - 90 days",   "oi-star"),
            new CropEntry("Sorghum",    "Kharif", new[]{"Punjab","Sindh","KPK"},                  new[]{"Sandy","Loamy","Clay"},          "Medium", "90 - 120 days",  "oi-target"),
            new CropEntry("Sesame",     "Kharif", new[]{"Punjab","Sindh"},                         new[]{"Sandy","Loamy"},                 "Medium", "80 - 100 days",  "oi-aperture"),

            // Rabi crops
            new CropEntry("Wheat",      "Rabi",   new[]{"Punjab","Sindh","KPK","Balochistan","ICT","AJK","GB"}, new[]{"Loamy","Clay","Silt"}, "High",   "120 - 150 days", "oi-layers"),
            new CropEntry("Barley",     "Rabi",   new[]{"Punjab","KPK","Balochistan"},             new[]{"Loamy","Sandy"},                 "Medium", "90 - 120 days",  "oi-list"),
            new CropEntry("Chickpea",   "Rabi",   new[]{"Punjab","KPK","Balochistan"},             new[]{"Loamy","Sandy","Clay"},          "High",   "90 - 120 days",  "oi-puzzle-piece"),
            new CropEntry("Lentil",     "Rabi",   new[]{"Punjab","KPK"},                           new[]{"Loamy","Clay"},                  "High",   "100 - 130 days", "oi-heart"),
            new CropEntry("Mustard",    "Rabi",   new[]{"Punjab","Sindh","KPK"},                  new[]{"Loamy","Sandy"},                 "Medium", "100 - 130 days", "oi-fire"),
            new CropEntry("Potato",     "Rabi",   new[]{"Punjab","KPK","Balochistan"},             new[]{"Loamy","Sandy","Silt"},          "High",   "90 - 120 days",  "oi-grid-three-up"),
            new CropEntry("Onion",      "Rabi",   new[]{"Punjab","Sindh","Balochistan"},           new[]{"Loamy","Sandy"},                 "Medium", "120 - 150 days", "oi-ellipses"),

            // Year-round crops
            new CropEntry("Tomato",     "Year-Round", new[]{"Punjab","Sindh","KPK","Balochistan"}, new[]{"Loamy","Sandy","Silt"},          "High",   "60 - 90 days",   "oi-badge"),
            new CropEntry("Chili",      "Year-Round", new[]{"Punjab","Sindh"},                     new[]{"Loamy","Sandy"},                 "Medium", "90 - 120 days",  "oi-fire"),
            new CropEntry("Spinach",    "Year-Round", new[]{"Punjab","Sindh","KPK"},              new[]{"Loamy","Clay","Silt"},           "High",   "30 - 45 days",   "oi-leaf" ),
            new CropEntry("Carrot",     "Year-Round", new[]{"Punjab","KPK","Balochistan"},        new[]{"Loamy","Sandy"},                 "High",   "70 - 90 days",   "oi-graph"),
        };

        public List<CropRecommendation> GetRecommendations(string region, string season, string soilType)
        {
            return _crops
                .Where(c =>
                    c.Seasons.Contains(season, StringComparer.OrdinalIgnoreCase) &&
                    c.Regions.Contains(region, StringComparer.OrdinalIgnoreCase) &&
                    c.SoilTypes.Contains(soilType, StringComparer.OrdinalIgnoreCase))
                .Select(c => new CropRecommendation
                {
                    Name = c.Name,
                    Suitability = c.Suitability,
                    Duration = c.Duration,
                    IconClass = c.IconClass
                })
                .OrderByDescending(c => c.Suitability == "High" ? 2 : c.Suitability == "Medium" ? 1 : 0)
                .ToList();
        }

        private class CropEntry
        {
            public string Name { get; }
            public string[] Seasons { get; }
            public string[] Regions { get; }
            public string[] SoilTypes { get; }
            public string Suitability { get; }
            public string Duration { get; }
            public string IconClass { get; }

            public CropEntry(string name, string season, string[] regions, string[] soils,
                             string suitability, string duration, string iconClass)
            {
                Name = name;
                Seasons = new[] { season };
                Regions = regions;
                SoilTypes = soils;
                Suitability = suitability;
                Duration = duration;
                IconClass = iconClass;
            }
        }
    }
}
