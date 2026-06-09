using System;
using System.Collections.Generic;
using System.Linq;
using AgriSmart.Web.Models;

namespace AgriSmart.Web.Services
{
    public class AdvisoryService
    {
        private readonly List<AdvisoryRecord> _advisories = new List<AdvisoryRecord>
        {
            new AdvisoryRecord { Id = 1, Date = new DateTime(2025, 5, 20), CropName = "Rice", Description = "Apply nitrogen fertilizer in the morning.", Tag = "Fertilizer" },
            new AdvisoryRecord { Id = 2, Date = new DateTime(2025, 5, 18), CropName = "Cotton", Description = "Monitor for whitefly and spray neem oil.", Tag = "Pest Control" },
            new AdvisoryRecord { Id = 3, Date = new DateTime(2025, 5, 15), CropName = "Maize", Description = "Irrigation recommended after 7 days.", Tag = "Irrigation" },
            new AdvisoryRecord { Id = 4, Date = new DateTime(2025, 5, 12), CropName = "Wheat", Description = "Harvest crop when grains are hard.", Tag = "Harvesting" },
            new AdvisoryRecord { Id = 5, Date = new DateTime(2025, 5, 10), CropName = "Sugarcane", Description = "Apply potash fertilizer at tillering stage.", Tag = "Fertilizer" },
            new AdvisoryRecord { Id = 6, Date = new DateTime(2025, 5, 8), CropName = "Rice", Description = "Inspect fields for stem borer moths.", Tag = "Pest Control" },
            new AdvisoryRecord { Id = 7, Date = new DateTime(2025, 5, 5), CropName = "Cotton", Description = "Schedule drip irrigation every 5 days.", Tag = "Irrigation" },
            new AdvisoryRecord { Id = 8, Date = new DateTime(2025, 5, 3), CropName = "Soybean", Description = "Harvest pods when they turn yellow-brown.", Tag = "Harvesting" },
            new AdvisoryRecord { Id = 9, Date = new DateTime(2025, 4, 28), CropName = "Maize", Description = "Apply DAP fertilizer at sowing time.", Tag = "Fertilizer" },
            new AdvisoryRecord { Id = 10, Date = new DateTime(2025, 4, 25), CropName = "Wheat", Description = "Watch for rust disease on leaves.", Tag = "Pest Control" },
            new AdvisoryRecord { Id = 11, Date = new DateTime(2025, 4, 20), CropName = "Rice", Description = "Ensure standing water depth of 5 cm.", Tag = "Irrigation" },
            new AdvisoryRecord { Id = 12, Date = new DateTime(2025, 4, 15), CropName = "Sugarcane", Description = "Cut mature cane at ground level.", Tag = "Harvesting" },
            new AdvisoryRecord { Id = 13, Date = new DateTime(2025, 4, 10), CropName = "Wheat", Description = "Top-dress urea fertilizer before final irrigation.", Tag = "Fertilizer" },
            new AdvisoryRecord { Id = 14, Date = new DateTime(2025, 4, 8), CropName = "Potato", Description = "Spray fungicide to prevent late blight disease.", Tag = "Pest Control" },
            new AdvisoryRecord { Id = 15, Date = new DateTime(2025, 4, 5), CropName = "Cotton", Description = "First irrigation 30-35 days after sowing.", Tag = "Irrigation" },
            new AdvisoryRecord { Id = 16, Date = new DateTime(2025, 4, 2), CropName = "Maize", Description = "Harvest maize when husks turn paper-like and grains are dry.", Tag = "Harvesting" },
            new AdvisoryRecord { Id = 17, Date = new DateTime(2025, 3, 28), CropName = "Sugarcane", Description = "Apply nitrogenous fertilizer in three split doses.", Tag = "Fertilizer" },
            new AdvisoryRecord { Id = 18, Date = new DateTime(2025, 3, 25), CropName = "Tomato", Description = "Use yellow sticky cards to capture whiteflies.", Tag = "Pest Control" },
            new AdvisoryRecord { Id = 19, Date = new DateTime(2025, 3, 22), CropName = "Rice", Description = "Maintain water level during tillering stage.", Tag = "Irrigation" },
            new AdvisoryRecord { Id = 20, Date = new DateTime(2025, 3, 18), CropName = "Wheat", Description = "Harvest when grain moisture content drops below 14%.", Tag = "Harvesting" },
            new AdvisoryRecord { Id = 21, Date = new DateTime(2025, 3, 15), CropName = "Citrus", Description = "Apply compost and zinc sulfate around tree basins.", Tag = "Fertilizer" },
            new AdvisoryRecord { Id = 22, Date = new DateTime(2025, 3, 12), CropName = "Mango", Description = "Spray against hopper insects during flowering stage.", Tag = "Pest Control" },
            new AdvisoryRecord { Id = 23, Date = new DateTime(2025, 3, 9), CropName = "Chilli", Description = "Irrigate lightly at flowering to prevent flower drop.", Tag = "Irrigation" },
            new AdvisoryRecord { Id = 24, Date = new DateTime(2025, 3, 5), CropName = "Onion", Description = "Harvest onions when 50% of tops fall over.", Tag = "Harvesting" },
            new AdvisoryRecord { Id = 25, Date = new DateTime(2025, 3, 2), CropName = "Sunflower", Description = "Apply nitrogen-phosphorus compound fertilizer.", Tag = "Fertilizer" },
            new AdvisoryRecord { Id = 26, Date = new DateTime(2025, 2, 28), CropName = "Cotton", Description = "Treat seeds with suitable pesticide before sowing.", Tag = "Pest Control" },
            new AdvisoryRecord { Id = 27, Date = new DateTime(2025, 2, 25), CropName = "Tomato", Description = "Drip irrigation recommended to avoid root rot.", Tag = "Irrigation" },
            new AdvisoryRecord { Id = 28, Date = new DateTime(2025, 2, 22), CropName = "Wheat", Description = "Apply second dose of urea at jointing stage.", Tag = "Fertilizer" },
            new AdvisoryRecord { Id = 29, Date = new DateTime(2025, 2, 18), CropName = "Rice", Description = "Harvest when 80-85% of grains turn straw-colored.", Tag = "Harvesting" },
            new AdvisoryRecord { Id = 30, Date = new DateTime(2025, 2, 15), CropName = "Maize", Description = "Inspect leaves for fall armyworm larvae.", Tag = "Pest Control" },
            new AdvisoryRecord { Id = 31, Date = new DateTime(2025, 2, 12), CropName = "Sugarcane", Description = "Irrigate crop at 10-12 days interval in winter.", Tag = "Irrigation" },
            new AdvisoryRecord { Id = 32, Date = new DateTime(2025, 2, 8), CropName = "Potato", Description = "Dehaulm potato crop 10 days before harvesting.", Tag = "Harvesting" },
        };

        public List<string> GetCropNames()
        {
            return _advisories.Select(a => a.CropName).Distinct().OrderBy(c => c).ToList();
        }

        public List<AdvisoryRecord> Search(DateTime? dateFrom, DateTime? dateTo, string crop)
        {
            var query = _advisories.AsEnumerable();

            if (dateFrom.HasValue)
                query = query.Where(a => a.Date.Date >= dateFrom.Value.Date);

            if (dateTo.HasValue)
                query = query.Where(a => a.Date.Date <= dateTo.Value.Date);

            if (!string.IsNullOrWhiteSpace(crop) && !crop.Equals("All Crops", StringComparison.OrdinalIgnoreCase))
                query = query.Where(a => a.CropName.Equals(crop, StringComparison.OrdinalIgnoreCase));

            return query.OrderByDescending(a => a.Date).ToList();
        }

        public void AddAdvisory(AdvisoryRecord record)
        {
            if (record == null) return;
            record.Id = _advisories.Any() ? _advisories.Max(a => a.Id) + 1 : 1;
            _advisories.Add(record);
        }

        public void UpdateAdvisory(AdvisoryRecord record)
        {
            if (record == null) return;
            var existing = _advisories.FirstOrDefault(a => a.Id == record.Id);
            if (existing != null)
            {
                existing.CropName = record.CropName;
                existing.Date = record.Date;
                existing.Description = record.Description;
                existing.Tag = record.Tag;
            }
        }

        public void DeleteAdvisory(int id)
        {
            var existing = _advisories.FirstOrDefault(a => a.Id == id);
            if (existing != null)
            {
                _advisories.Remove(existing);
            }
        }

        public int TotalAdvisories => _advisories.Count;
    }
}
