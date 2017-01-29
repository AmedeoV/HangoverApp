using System;
using System.Diagnostics;
using Android.Webkit;
using HangoverApp.Helpers;
using Plugin.Connectivity;
using Plugin.SecureStorage;
using Xamarin.Forms;
using Xamarinos.AdMob.Forms;

namespace HangoverApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            Device.BeginInvokeOnMainThread(() => {
                DisplayAlert("Welcome", "Hey in order to use the app you will need a Just-Eat account, sign in or create one!", "OK");
            });
            var myBanner = new AdBanner()
            {
                VerticalOptions = LayoutOptions.Start
            };
            //Set Your AdMob Key
            myBanner.AdID = "ca-app-pub-3564256941949898/8465093569";
            StackLayout objStackLayout = new StackLayout()
            {
            };
            //
            Xamarin.Forms.WebView objWebView1 = new Xamarin.Forms.WebView();
            objWebView1.HeightRequest = 300;
            objStackLayout.Children.Add(myBanner);
            objStackLayout.Children.Add(objWebView1);

            //
            UrlWebViewSource objUrlToNavigateTo = new UrlWebViewSource()
            {
                Url = "https://www.just-eat.co.uk/account/login"
            };

            objWebView1.Source = objUrlToNavigateTo;
            objWebView1.VerticalOptions = LayoutOptions.FillAndExpand;
            objWebView1.HorizontalOptions = LayoutOptions.FillAndExpand;
            objWebView1.Navigating += ConfirmNavigationAndSendToWebBrowserApp;
            //
            //
            this.Content = objStackLayout;
        }


        public static async void ConfirmNavigationAndSendToWebBrowserApp(object sender, WebNavigatingEventArgs e)
        {
            Page page = new Page();

            if (CrossConnectivity.Current.IsConnected)
            {

                if (e.Url.StartsWith("http"))
                {
                    try
                    {
                        var cookieHeader = CookieManager.Instance.GetCookie(e.Url);
                        Saveset(cookieHeader);
                        WebOperations operation = new WebOperations();
                        var isLoggedIn = operation.TryToLogIn(cookieHeader);
                        if (isLoggedIn)
                        {
                            await page.Navigation.PushModalAsync(new MainListPage());
                            if (Device.OS == TargetPlatform.Android)
                                Application.Current.MainPage = new MainListPage();
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
            }
            else
            {
                await page.DisplayAlert("Connection", "Connection lost, please check your connection and retry", "OK");
            }
        }

        private void SlowMethod()
        {
          
            
        }
        protected static void Saveset(string cookie)
        {
            CrossSecureStorage.Current.SetValue("myCookie", cookie);
        }

        protected string Retrieveset()
        {
            return CrossSecureStorage.Current.GetValue("myCookie");
        }

    }

}
