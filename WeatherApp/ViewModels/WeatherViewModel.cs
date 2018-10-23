using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using WeatherApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WeatherApp.ViewModels
{
    public class WeatherViewModel:BaseViewModel
    {
        private WeatherForecastRoot _forcast;
        public WeatherForecastRoot Forcast 
        {
            get { return _forcast; } 
            set
            {
                _forcast = value;
                OnPropertyChanged();
            }
        }
        private WeatherRoot _weather;
        public WeatherRoot  Weather 
        {
            get { return _weather; } 
            set
            {
                _weather = value;
                OnPropertyChanged();
            }
        }

        private List<WeatherItem> _weatheritems;
        public List<WeatherItem> WeatherItems{
            get { return _weatheritems; }
            set
            {
                _weatheritems = value;
                OnPropertyChanged();
            }
        }
        private WeatherItem _weatheritem;
        public WeatherItem WeatherItem
        {
            get { return _weatheritem; }
            set
            {
                _weatheritem = value;
                OnPropertyChanged();
            }
        }

        public Command LoadItemsCommand { get; set; }
        public WeatherViewModel()
        {
            Forcast = new WeatherForecastRoot();
            Weather = new WeatherRoot();
            WeatherItems = new List<WeatherItem>();

            WeatherItems.Add(new WeatherItem { id = 1, backgroundimage = "forest_sunny.png", desc = "SUNNY", colorhex = "#47AB2F", icon = "clear.png" });
            WeatherItems.Add(new WeatherItem { id = 2, backgroundimage = "forest_cloudy.png", desc = "CLOUDY", colorhex = "#54717A", icon = "partlysunny.png" });
            WeatherItems.Add(new WeatherItem { id = 3, backgroundimage = "forest_rainy.png", desc = "RAINY", colorhex = "#57575D", icon = "rain.png" });

            GetWeatherCommand.Execute(null);

        }

        
        ICommand getWeather;
        public ICommand GetWeatherCommand =>
                getWeather ??
                (getWeather = new Command(async () => await ExecuteGetWeatherCommand()));
        async Task ExecuteGetWeatherCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var hasPermission = await CheckPermissions();
                if (!hasPermission)
                    return;

                var position = await Geolocation.GetLastKnownLocationAsync();

                if (position == null)
                {
                    // get full location if not cached.
                    position = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                Weather = await DataStore.GetCurrentWeatherAsync(new Coord{Latitude= position.Latitude, Longitude= position.Longitude });

                Forcast = await DataStore.GetForcastAsync(Weather.CityId);
                Forcast.Items = Forcast.Items.GroupBy(x => x.DisplayDate).Select(g => g.First()).ToList();
                foreach (var i in Forcast.Items)
                {
                    var rainy = WeatherItems.Where(x => x.id == 3).FirstOrDefault(); //Rainy
                    var cloudy = WeatherItems.Where(x => x.id == 2).FirstOrDefault(); //Cloudy
                    var sunny = WeatherItems.Where(x => x.id == 1).FirstOrDefault(); //Sunny
                    switch (i.Weather.First().Id)
                    {
                        case int n when (n >= 200 && n <= 232):
                            i.DisplayIcon = rainy.icon;
                            break;
                        case int n when (n >= 300 && n <= 321):
                            i.DisplayIcon = rainy.icon;
                            break;
                        case int n when (n >= 500 && n <= 521):
                            i.DisplayIcon = rainy.icon;
                            break;
                        case int n when (n >= 600 && n <= 622):
                            i.DisplayIcon = rainy.icon;
                            break;
                        case int n when (n >= 700 && n <= 781):
                            i.DisplayIcon = cloudy.icon;
                            break;
                        case int n when (n >= 801 && n <= 804):
                            i.DisplayIcon = cloudy.icon;
                            break;
                        case 800:
                            i.DisplayIcon = sunny.icon;
                            break;
                        default:
                            break;
                    }
                }
                //Forcast.Items
                if(Weather!=null)
                    Configure();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        void Configure()
        {
            var rainy = WeatherItems.Where(x => x.id == 3).FirstOrDefault(); //Rainy
            var cloudy = WeatherItems.Where(x => x.id == 2).FirstOrDefault(); //Cloudy
            var sunny = WeatherItems.Where(x => x.id == 1).FirstOrDefault(); //Sunny

            WeatherItem = new WeatherItem();
           
            switch (Weather.Weather.First().Id)
            {
                case int n when (n >= 200 && n <= 232):
                    WeatherItem = rainy;
                    break;
                case int n when (n >= 300 && n <= 321):
                    WeatherItem = rainy; //Rainy
                    break;
                case int n when (n >= 500 && n <= 521):
                    WeatherItem = rainy; //Rainy
                    break;
                case int n when (n >= 600 && n <= 622):
                    WeatherItem = rainy; //Rainy
                    break;
                case int n when (n >= 700 && n <= 781):
                    WeatherItem = cloudy;
                    break;
                case int n when (n >= 801 && n <= 804):
                    WeatherItem = cloudy;
                    break;
                case 800:
                    WeatherItem = sunny;
                    break;
                default:
                    break;
            }
            WeatherItem.mintemp = Weather.MainWeather.MinTemperature;
            WeatherItem.maxtemp = Weather.MainWeather.MaxTemperature;
            WeatherItem.temp = Weather.MainWeather.Temperature;
        }

        protected async Task<bool> CheckPermissions()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {

                    var title = "Location Permission";
                    var question = "To get your current location permission is required. Please go into Settings and turn on Location for the app.";
                    var positive = "Settings";
                    var negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }

                    return false;
                }

                request = true;
            }

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                if (newStatus.ContainsKey(Permission.Location) && newStatus[Permission.Location] != PermissionStatus.Granted)
                {
                    var title = "Location Permission";
                    var question = "To get your current location permission is required.";
                    var positive = "Settings";
                    var negative = "Maybe Later";
                    var task = Application.Current.MainPage.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                    return false;
                }
            }

            return true;
        }
    }
}
