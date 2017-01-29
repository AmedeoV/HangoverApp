using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class CountryPage : ContentPage
    {

        Dictionary<string, string> _countryToUrl = new Dictionary<string, string>
        {
            { "Ireland", "https://www.just-eat.ie/" },
            { "Italy", "https://www.justeat.it/" },
            { "UK", CrossSecureStorage.Current.GetValue("myCountry") },
        };
        public CountryPage()
        {
            BackgroundImage = "LoginScreen.png";

            Picker picker = new Picker
            {
                Title = "Select your Country",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.Default,
                Scale = 2,
                BackgroundColor = Color.FromHex("#1976D2"),

            };

            foreach (string colorName in _countryToUrl.Keys)
            {
                picker.Items.Add(colorName);
            }

            picker.SelectedIndexChanged += (sender, args) =>
            {
                if (picker.SelectedIndex == -1)
                {
                }
                else
                {
                    string selectedCountry = picker.Items[picker.SelectedIndex];
                    string url = _countryToUrl[selectedCountry];
                    var myCountry = CrossSecureStorage.Current.SetValue("myCountry", url);

                }
            };

            var confirmButton = new Button()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                Scale = 1.5,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = "Confirm",
                BackgroundColor = Color.FromHex("#8BC34A"),
                InputTransparent = true,
                IsEnabled = true
            };

            confirmButton.Clicked += (sender, e) =>
           {
               if (picker.SelectedIndex != -1)
               {
                   if (Device.OS == TargetPlatform.Android)
                       Application.Current.MainPage = new LoginPage();
               }
               else
               {
                   DisplayAlert("Error", "Please select your Country", "OK");
               }

           };

            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    picker,
                    confirmButton
                    //boxView
                }
            };


        }

    }
}
