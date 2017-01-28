using HangoverApp.Controls;
using HangoverApp.Style;
using HangoverApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HangoverApp.Helpers;
using HangoverApp.Models;
using Plugin.Connectivity;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class MainListPage : ContentPage
    {
        public MainListPage()
        {
            
            BindingContext = new LoginViewModel(this);
            var oldPostcode = "";

            var activityIndicator = new ActivityIndicator
            {
                Color = Color.FromHex("#1976D2"),
                Scale = 2
            };

            if (!String.IsNullOrEmpty(CrossSecureStorage.Current.GetValue("postcode")))
            {
                oldPostcode = CrossSecureStorage.Current.GetValue("postcode");
            }
            

            activityIndicator.IsRunning = true;
            activityIndicator.IsEnabled = true;
            activityIndicator.BindingContext = this;
            activityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");
            activityIndicator.VerticalOptions = LayoutOptions.CenterAndExpand;
            activityIndicator.HorizontalOptions = LayoutOptions.CenterAndExpand;


            BackgroundImage = "LoginScreen.png";


            var layout = new StackLayout { Padding = 20 };
            layout.Children.Add(activityIndicator);
            var button = new Button()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                Scale = 1.5,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = "Feed Me!",
                BackgroundColor = Color.FromHex("#8BC34A"),
                InputTransparent = true,
                IsEnabled = true
            };

            var textBox = new Entry()
            {
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = 1,
                BackgroundColor = Color.Black,
                Placeholder = "Enter your postcode...",
                Text = oldPostcode,
                PlaceholderColor = Color.Gray
            };

            var backgroundImage = new Image()
            {
                Aspect = Aspect.Fill,
                Source = FileImageSource.FromFile("LoginScreen.png")
            };


            var loadingLabel = new Label()
            {
                Text = "Searching...",
                TextColor = Color.FromHex("#1976D2"),
                IsVisible = false,
                Scale = 4,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Italic
            };

            textBox.Completed += async (sender, e) =>
            {
                await SearchByPostcode(activityIndicator, textBox, button, loadingLabel, backgroundImage);
            };

            layout.Children.Add(textBox);
            layout.Children.Add(button);
            layout.Children.Add(loadingLabel);

            var relativelayout = new RelativeLayout();

            relativelayout.Children.Add(backgroundImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            relativelayout.Children.Add(layout,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));



            button.Clicked += async (sender, e) =>
            {
                await SearchByPostcode(activityIndicator, textBox, button, loadingLabel, backgroundImage);
            };

            Content = new ScrollView { Content = relativelayout };

        }

        private async Task SearchByPostcode(ActivityIndicator activityIndicator, Entry textBox, Button button, Label loadingLabel, Image backgroundImage)
        {
            if (String.IsNullOrEmpty(textBox.Text))
            {
                await DisplayAlert("Error", "Please enter your postcode", "Re-try");
            }
            else if (!IsPostCode(textBox.Text))
            {
                await DisplayAlert("Error", "The postcode entered is not valid", "Re-try");
            }
            else
            {
                IsBusy = true;
                activityIndicator.IsVisible = true;
                activityIndicator.IsRunning = true;
                textBox.IsVisible = false;
                loadingLabel.IsVisible = true;
                button.IsVisible = false;
                CrossSecureStorage.Current.SetValue("postcode", textBox.Text);
                WebOperations operation = new WebOperations();
                var source =
                    await
                        operation.PostAction("https://www.just-eat.co.uk/search/do",
                            CrossSecureStorage.Current.GetValue("myCookie") + "", "", textBox.Text);
                Page page = new Page();
                await page.Navigation.PushModalAsync(new RestaurantsListPage(source));
                if (Device.OS == TargetPlatform.Android)
                    Application.Current.MainPage = new RestaurantsListPage(source);
            }
        }

        static public bool IsPostCode(string postcode)
        {
            return (
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][0-9][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][0-9][A-HJKS-UWa-hjks-uw][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][A-Za-z][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[Gg][Ii][Rr][]*0[Aa][Aa]$)")
                );
        }
    }
}
