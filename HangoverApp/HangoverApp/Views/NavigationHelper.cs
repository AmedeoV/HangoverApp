using System;
using Android.Webkit;
using HangoverApp.Helpers;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class NavigationHelper
    {
        public static async void ConfirmNavigationAndSendToWebBrowserApp(object sender, WebNavigatingEventArgs e, ProgressBar activityIndicator)
        {
            Page page = new Page();

            var valueArray = e.Url.Split('/');
            if (valueArray.Length == 4 && valueArray[3].Contains("restaurants"))
            {
                e.Cancel = true;
                activityIndicator.IsVisible = true;
                await activityIndicator.ProgressTo(1, 950, Easing.Linear);
                var cookie = CookieManager.Instance.GetCookie(e.Url);
                CrossSecureStorage.Current.SetValue("myCookie", cookie);
                WebOperations operation = new WebOperations();
                var source = await operation.AccessTheWebAsync(e.Url, cookie, "");

                await page.Navigation.PushModalAsync(new InfoPage(source));
                if (Device.OS == TargetPlatform.Android)
                    Application.Current.MainPage = new InfoPage(source);
            }



            if (valueArray.Length == 5 && e.Url.Contains("reviews"))
            {
                e.Cancel = true;
                activityIndicator.IsVisible = true;
                await activityIndicator.ProgressTo(1, 950, Easing.Linear);
                var cookie = CookieManager.Instance.GetCookie(e.Url);
                CrossSecureStorage.Current.SetValue("myCookie", cookie);
                WebOperations operation = new WebOperations();
                var source = await operation.AccessTheWebAsync(e.Url, cookie, "");

                await page.Navigation.PushModalAsync(new ReviewsPage(source));
                if (Device.OS == TargetPlatform.Android)
                    Application.Current.MainPage = new ReviewsPage(source);
            }

            if (valueArray.Length == 5 && valueArray[4] == "menu")
            {
                e.Cancel = true;
                activityIndicator.IsVisible = true;
                await activityIndicator.ProgressTo(1, 950, Easing.Linear);
                var cookie = CookieManager.Instance.GetCookie(e.Url);
                CrossSecureStorage.Current.SetValue("myCookie", cookie);
                //var cookie = CrossSecureStorage.Current.GetValue("myCookie");
                WebOperations operation = new WebOperations();
                var source = await operation.AccessTheWebAsync(e.Url, cookie, "");
                await page.Navigation.PushModalAsync(new RestaurantMenuPage(source));
                if (Device.OS == TargetPlatform.Android)
                    Application.Current.MainPage = new RestaurantMenuPage(source);
            }

            if (valueArray.Length == 4 && valueArray[3] == "")
            {
                e.Cancel = true;
            }
        }

        public async void CheckOtherRestaurantsEvent(object sender, EventArgs ea, ProgressBar activityIndicator, Xamarin.Forms.WebView objWebView1)
        {
            objWebView1.IsEnabled = false;
            activityIndicator.IsVisible = true;
            await activityIndicator.ProgressTo(1, 950, Easing.Linear);
            WebOperations operation = new WebOperations();
            var source = await operation.PostAction("https://www.just-eat.co.uk/search/do", CrossSecureStorage.Current.GetValue("myCookie"), "", CrossSecureStorage.Current.GetValue("postcode"));

            Page page = new Page();
            await page.Navigation.PushModalAsync(new RestaurantsListPage(source));
            if (Device.OS == TargetPlatform.Android)
                Application.Current.MainPage = new RestaurantsListPage(source);
        }
    }
}