using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AgriSmart.Web.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AgriSmart.Web.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<WeatherService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiKey = configuration.GetValue<string>("WeatherApi:ApiKey");
            _baseUrl = configuration.GetValue<string>("WeatherApi:BaseUrl") ?? "https://api.openweathermap.org/data/2.5/";
            _logger = logger;
        }

        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                city = "Islamabad";
            }

            // Check if API Key is configured and valid
            if (!string.IsNullOrWhiteSpace(_apiKey) && !_apiKey.Equals("YOUR_KEY_HERE", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var response = await _httpClient.GetAsync($"{_baseUrl}weather?q={city},PK&appid={_apiKey}&units=metric");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        using var doc = JsonDocument.Parse(json);
                        var root = doc.RootElement;

                        var data = new WeatherData
                        {
                            City = root.GetProperty("name").GetString(),
                            Province = GetProvinceForCity(city),
                            Temperature = (int)Math.Round(root.GetProperty("main").GetProperty("temp").GetDouble()),
                            Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                            Pressure = root.GetProperty("main").GetProperty("pressure").GetInt32(),
                            WindSpeed = (int)Math.Round(root.GetProperty("wind").GetProperty("speed").GetDouble() * 3.6), // Convert m/s to km/h
                            Condition = root.GetProperty("weather")[0].GetProperty("main").GetString()
                        };

                        // Fetch forecast
                        var forecastResponse = await _httpClient.GetAsync($"{_baseUrl}forecast?q={city},PK&appid={_apiKey}&units=metric");
                        if (forecastResponse.IsSuccessStatusCode)
                        {
                            var forecastJson = await forecastResponse.Content.ReadAsStringAsync();
                            using var forecastDoc = JsonDocument.Parse(forecastJson);
                            var forecastRoot = forecastDoc.RootElement;
                            var list = forecastRoot.GetProperty("list");

                            // Build Hourly Forecast (take first 5 intervals)
                            for (int i = 0; i < Math.Min(5, list.GetArrayLength()); i++)
                            {
                                var item = list[i];
                                var dtText = item.GetProperty("dt_txt").GetString();
                                var dt = DateTime.Parse(dtText);
                                var temp = (int)Math.Round(item.GetProperty("main").GetProperty("temp").GetDouble());
                                var cond = item.GetProperty("weather")[0].GetProperty("main").GetString();

                                data.HourlyForecasts.Add(new HourlyForecast
                                {
                                    Time = dt.ToString("h tt"),
                                    Temperature = temp,
                                    IconClass = GetWeatherIcon(cond)
                                });
                            }

                            // Build 5 Day Forecast (take 1 reading per day around 12:00 PM)
                            var dailyData = new Dictionary<string, (int max, int min, string cond)>();
                            for (int i = 0; i < list.GetArrayLength(); i++)
                            {
                                var item = list[i];
                                var dtText = item.GetProperty("dt_txt").GetString();
                                var dt = DateTime.Parse(dtText);
                                var dateKey = dt.ToString("yyyy-MM-dd");

                                // Skip today's remaining forecast
                                if (dt.Date == DateTime.Today) continue;

                                var temp = (int)Math.Round(item.GetProperty("main").GetProperty("temp").GetDouble());
                                var cond = item.GetProperty("weather")[0].GetProperty("main").GetString();

                                if (!dailyData.ContainsKey(dateKey))
                                {
                                    dailyData[dateKey] = (temp, temp, cond);
                                }
                                else
                                {
                                    var current = dailyData[dateKey];
                                    dailyData[dateKey] = (
                                        Math.Max(current.max, temp),
                                        Math.Min(current.min, temp),
                                        dt.Hour == 12 ? cond : current.cond
                                    );
                                }
                            }

                            int dayCount = 0;
                            foreach (var kp in dailyData)
                            {
                                if (dayCount >= 5) break;
                                var date = DateTime.Parse(kp.Key);
                                data.DailyForecasts.Add(new DailyForecast
                                {
                                    Day = date.ToString("ddd, dd MMM"),
                                    MaxTemp = kp.Value.max,
                                    MinTemp = kp.Value.min,
                                    IconClass = GetWeatherIcon(kp.Value.cond)
                                });
                                dayCount++;
                            }

                            return data;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to retrieve live weather data for {City}. Falling back to mock data.", city);
                }
            }

            // Fallback to high-quality mockup-style mock data
            return GetMockWeatherData(city);
        }

        private WeatherData GetMockWeatherData(string city)
        {
            var random = new Random(city.GetHashCode());
            var baseTemp = random.Next(22, 38);
            var condition = GetRandomCondition(random);
            var humidity = random.Next(40, 85);
            var wind = random.Next(5, 25);
            var pressure = random.Next(1008, 1018);

            var data = new WeatherData
            {
                City = city,
                Province = GetProvinceForCity(city),
                Temperature = baseTemp,
                Condition = condition,
                Humidity = humidity,
                WindSpeed = wind,
                Pressure = pressure
            };

            // Generate hourly forecast
            var now = DateTime.Now;
            var hourlyHours = new[] { 10, 13, 16, 19, 22 };
            for (int i = 0; i < hourlyHours.Length; i++)
            {
                var hour = hourlyHours[i];
                var forecastTime = new DateTime(now.Year, now.Month, now.Day, hour, 0, 0);
                var tempOffset = hour switch
                {
                    10 => -2,
                    13 => 1,
                    16 => 2,
                    19 => -2,
                    22 => -5,
                    _ => 0
                };

                var hourlyCond = (hour >= 19) ? "Clear" : condition;

                data.HourlyForecasts.Add(new HourlyForecast
                {
                    Time = forecastTime.ToString("h tt"),
                    Temperature = baseTemp + tempOffset,
                    IconClass = GetWeatherIcon(hourlyCond)
                });
            }

            // Generate 5-day forecast
            for (int i = 1; i <= 5; i++)
            {
                var day = now.AddDays(i);
                var dayCond = GetRandomCondition(new Random(city.GetHashCode() + i));
                data.DailyForecasts.Add(new DailyForecast
                {
                    Day = day.ToString("ddd, dd MMM"),
                    MaxTemp = baseTemp + random.Next(1, 5),
                    MinTemp = baseTemp - random.Next(4, 9),
                    IconClass = GetWeatherIcon(dayCond)
                });
            }

            return data;
        }

        private string GetProvinceForCity(string city)
        {
            var punjab = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Lahore", "Faisalabad", "Multan", "Rawalpindi", "Gujranwala", "Sialkot", "Bahawalpur",
                "Sargodha", "Sahiwal", "Gujrat", "Kasur", "Jhang", "Dera Ghazi Khan", "Sheikhupura", "Ludhiana"
            };

            var sindh = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Karachi", "Hyderabad", "Sukkur", "Larkana", "Mirpur Khas", "Nawabshah", "Jacobabad"
            };

            var kpk = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Peshawar", "Abbottabad", "Mardan", "Mingora", "Dera Ismail Khan", "Kohat"
            };

            var balochistan = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Quetta", "Khuzdar", "Turbat"
            };

            if (punjab.Contains(city)) return "Punjab";
            if (sindh.Contains(city)) return "Sindh";
            if (kpk.Contains(city)) return "KPK";
            if (balochistan.Contains(city)) return "Balochistan";
            if (city.Equals("Islamabad", StringComparison.OrdinalIgnoreCase)) return "ICT";
            if (city.Equals("Muzaffarabad", StringComparison.OrdinalIgnoreCase) || city.Equals("Mirpur AJK", StringComparison.OrdinalIgnoreCase)) return "AJK";
            if (city.Equals("Gilgit", StringComparison.OrdinalIgnoreCase) || city.Equals("Skardu", StringComparison.OrdinalIgnoreCase)) return "GB";

            return "Pakistan";
        }

        private string GetRandomCondition(Random random)
        {
            var conditions = new[] { "Sunny", "Partly Cloudy", "Cloudy", "Rainy" };
            return conditions[random.Next(conditions.Length)];
        }

        private string GetWeatherIcon(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition)) return "oi-sun";

            return condition.ToLower() switch
            {
                var c when c.Contains("sun") || c.Contains("clear") => "oi-sun",
                var c when c.Contains("partly") => "oi-cloud", // fallback partly cloudy
                var c when c.Contains("cloud") => "oi-cloud",
                var c when c.Contains("rain") || c.Contains("drizzle") || c.Contains("shower") => "oi-rain",
                var c when c.Contains("snow") => "oi-bolt",
                _ => "oi-sun"
            };
        }
    }
}
