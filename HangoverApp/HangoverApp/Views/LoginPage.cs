using Android.Content;
using Android.Preferences;
using Android.Webkit;
using HangoverApp.ViewModels;
using Plugin.SecureStorage;
using System;
using System.Diagnostics;
using System.Linq;
using Android.Util;
using HangoverApp.Helpers;
using HangoverApp.Views;
using Xamarin.Forms;

namespace HangoverApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            Device.BeginInvokeOnMainThread(() => {
                DisplayAlert("Hey " + "you", "Welcome", "OK");
            });
            StackLayout objStackLayout = new StackLayout()
            {
            };
            //
            Xamarin.Forms.WebView objWebView1 = new Xamarin.Forms.WebView();
            objWebView1.HeightRequest = 300;
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
                        page.Navigation.PushModalAsync(new MainListPage());
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
