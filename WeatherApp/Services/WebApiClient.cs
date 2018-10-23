using WeatherApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WeatherApp.Services
{
    public enum Units
    {
        Imperial,
        Metric
    }

    public class WebApiClient : IDataStore<WeatherRoot>
    {
        public static string _apikey="64ff13a9d9426de173604d61bbdee439";
       const string baseurl = "http://api.openweathermap.org/data/2.5/";
        public WebApiClient()
        {

        }

        public async Task<WeatherForecastRoot> GetForcastAsync(int id, Units units = Units.Imperial)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(baseurl + "forecast?id={0}&units={1}&appid={2}", id, units.ToString().ToLower(), _apikey);
                var json = await client.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<WeatherForecastRoot>(json);
            }
        }

        public async Task<WeatherRoot> GetCurrentWeatherAsync(Coord coord, Units units = Units.Imperial)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(baseurl+"weather?lat={0}&lon={1}&units={2}&appid={3}", coord.Latitude.ToString(),
                                        coord.Longitude.ToString(), units.ToString().ToLower(), _apikey);
                var json = await client.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<WeatherRoot>(json);
            }
        }
    }
}
