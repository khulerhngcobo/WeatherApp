using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public interface IDataStore<T>
    {
       // Task<bool> AddItemAsync(T item);
        //Task<bool> UpdateItemAsync(T item);
        //Task<bool> DeleteItemAsync(string id);
       // Task<T> GetItemAsync(string id);
       // Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

        Task<T> GetCurrentWeatherAsync(Coord coord, Units units = Units.Imperial);
        Task<WeatherForecastRoot> GetForcastAsync(int id, Units units = Units.Imperial);
    }
}
