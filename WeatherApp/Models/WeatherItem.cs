using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WeatherApp.Helpers;
using WeatherApp.ViewModels;

namespace WeatherApp.Models
{
    public class WeatherItem : BaseViewModel
    {
        public int id { get; set; }
        public string desc { get; set; }
        public string colorhex { get; set; }
        public string icon { get; set; }
        public string backgroundimage { get; set; }
        public double mintemp { get; set; }
        public double maxtemp { get; set; }
        public double temp { get; set; }
        public WeatherItem()
        {
        }
    }

    public class Coord : BaseViewModel
    {
        [JsonProperty("lon")]
        public double Longitude { get; set; } = 0;

        [JsonProperty("lat")]
        public double Latitude { get; set; } = 0;
    }

    public class Sys
    {

        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;
    }

    public class Weather:BaseViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; } = 0;

        [JsonProperty("main")]
        public string Main { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("icon")]
        public string Icon { get; set; } = string.Empty;
    }

    public class Main : BaseViewModel
    {
        [JsonProperty("temp")]
        public double Temperature { get; set; } = 0;
        [JsonProperty("pressure")]
        public double Pressure { get; set; } = 0;

        [JsonProperty("humidity")]
        public double Humidity { get; set; } = 0;
        [JsonProperty("temp_min")]
        public double MinTemperature { get; set; } = 0;

        [JsonProperty("temp_max")]
        public double MaxTemperature { get; set; } = 0;
    }

    public class Wind : BaseViewModel
    {
        [JsonProperty("speed")]
        public double Speed { get; set; } = 0;

        [JsonProperty("deg")]
        public double WindDirectionDegrees { get; set; } = 0;

    }

    public class Clouds : BaseViewModel
    {

        [JsonProperty("all")]
        public int CloudinessPercent { get; set; } = 0;
    }

    public class WeatherRoot:BaseViewModel
    {
        [JsonProperty("coord")]
        public Coord Coordinates { get; set; } = new Coord();

        [JsonProperty("sys")]
        public Sys System { get; set; } = new Sys();

        [JsonProperty("weather")]
        public List<Weather> Weather { get; set; } = new List<Weather>();

        [JsonProperty("main")]
        public Main MainWeather { get; set; } = new Main();

        [JsonProperty("wind")]
        public Wind Wind { get; set; } = new Wind();

        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; } = new Clouds();

        [JsonProperty("id")]
        public int CityId { get; set; } = 0;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("dt_txt")]
        public string Date { get; set; } = string.Empty;

        [JsonIgnore]
        public string DisplayDate => DateTime.Parse(Date).ToLocalTime().ToString("dddd");
        [JsonIgnore]
        public string DisplayTemp => $" {MainWeather?.Temperature ?? 0}° {Weather?[0]?.Main ?? string.Empty}";
        [JsonIgnore]
        public string DisplayIcon { get; set; }
        // public string DisplayIcon => $"http://openweathermap.org/img/w/{Weather?[0]?.Icon}.png";
    }

    public class WeatherForecastRoot : BaseViewModel
    {
        [JsonProperty("city")]
        public City City { get; set; }
        [JsonProperty("cod")]
        public string Vod { get; set; }
        [JsonProperty("message")]
        public double Message { get; set; }
        [JsonProperty("cnt")]
        public int Cnt { get; set; }
        [JsonProperty("list")]
        public List<WeatherRoot> Items { get; set; }

    }
    public class DailyWeatherForecastRoot : BaseViewModel
    {
        [JsonProperty("city")]
        public City City { get; set; }
        [JsonProperty("cod")]
        public string Vod { get; set; }
        [JsonProperty("message")]
        public double Message { get; set; }
        [JsonProperty("cnt")]
        public int Cnt { get; set; }
        [JsonProperty("list")]
        public List<DailyWeatherRoot> Items { get; set; }

    }

    public class DailyWeatherRoot : BaseViewModel
    {
        //[JsonProperty(ItemConverterType = typeof(UTCDateTimeConverter),PropertyName ="dt")]
      //  public DateTime Date { get; set; }

        [JsonProperty("temp")]
        public Temperature temperature { get; set; }

        [JsonProperty("coord")]
        public Coord Coordinates { get; set; } = new Coord();

        [JsonProperty("sys")]
        public Sys System { get; set; } = new Sys();

        [JsonProperty("weather")]
        public List<Weather> Weather { get; set; } = new List<Weather>();

        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; } = new Clouds();

       // [JsonIgnore]
       // public string DisplayDate => Date.ToLocalTime().ToString("g");
        [JsonIgnore]
        public string DisplayTemp => $"{temperature?.max ?? 0}° {Weather?[0]?.Main ?? string.Empty}";
        [JsonIgnore]
        public string DisplayIcon { get; set; }
        // public string DisplayIcon => $"http://openweathermap.org/img/w/{Weather?[0]?.Icon}.png";
    }
    public class Temperature:BaseViewModel
    {
        public int day { get; set; }
        public int min { get; set; }
        public int max { get; set; }
        public int night { get; set; }
        public int eve { get; set; }
        public int morn { get; set; }
    }

    public class City : BaseViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("coord")]
        public Coord Coord { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("population")]
        public int Population { get; set; }
        [JsonProperty("sys")]
        public Sys Sys { get; set; }
    }
}
