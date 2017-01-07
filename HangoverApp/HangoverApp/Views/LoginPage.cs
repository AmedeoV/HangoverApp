using Android.Content;
using Android.Preferences;
using Android.Webkit;
using HangoverApp.ViewModels;
using Plugin.SecureStorage;
using System;
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
            Page page = new Page();
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
            //
            //
            Button cmdButton1 = new Button();
            cmdButton1.Text = "I am logged in!";
            objStackLayout.Children.Add(cmdButton1);
            //
            

            cmdButton1.Clicked += ((o2, e2) =>
            {
                var urlWebViewSource = objWebView1.Source as UrlWebViewSource;
                if (urlWebViewSource != null && (urlWebViewSource.Url).Contains("login"))
                {
                    CookieManager.Instance.RemoveAllCookie();
                    page.DisplayAlert("Login Error", " In order to use this app you will need a JustEat account. Please log in and click on the button 'I am logged in!'", "Ok");
                }
                else
                {
                    var cookieHeader = CookieManager.Instance.GetCookie((objWebView1.Source as UrlWebViewSource).Url);
                    Saveset(cookieHeader);
                    WebOperations operation = new WebOperations();
                    var isLoggedIn = operation.TryToLogIn(cookieHeader);
                    if (isLoggedIn)
                    {
                        page.Navigation.PushModalAsync(new RootPage());
                        if (Device.OS == TargetPlatform.Android)
                            Application.Current.MainPage = new RootPage();
                    }
                    else
                    {
                        page.DisplayAlert("Login Error", " In order to use this app you will need a JustEat account. Please log in and click on the button 'I am logged in!'", "Ok");
                        page.Navigation.PushModalAsync(new LoginPage());
                        if (Device.OS == TargetPlatform.Android)
                            Application.Current.MainPage = new LoginPage();
                    }
                }

            });
            //
            //
            this.Content = objStackLayout;
        }

        protected void Saveset(string cookie)
        {
            CrossSecureStorage.Current.SetValue("myCookie", cookie);
        }

        protected string Retrieveset()
        {
            return CrossSecureStorage.Current.GetValue("myCookie");
        }

    }

}
