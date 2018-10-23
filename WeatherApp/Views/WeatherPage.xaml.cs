using System;
using System.Collections.Generic;
using WeatherApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPage : ContentPage
    {
        WeatherViewModel vm;
        public WeatherPage()
        {
            InitializeComponent();
            vm = new WeatherViewModel();
            BindingContext = vm;
        }
    }
}
