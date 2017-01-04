using Android.Content;
using Android.Preferences;
using Android.Webkit;
using HangoverApp.ViewModels;
using Plugin.SecureStorage;
using System;
using Xamarin.Forms;

namespace HangoverApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
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
            cmdButton1.Text = "Click this button when you logged in...";
            objStackLayout.Children.Add(cmdButton1);
            //
            cmdButton1.Clicked += ((o2, e2) =>
            {
                var cookieHeader = CookieManager.Instance.GetCookie((objWebView1.Source as UrlWebViewSource).Url);
                saveset(cookieHeader);
                string test = retrieveset();
            });
            //
            //
            this.Content = objStackLayout;
        }

        protected void saveset(string cookie)
        {
            CrossSecureStorage.Current.SetValue("myCookie", cookie);
        }

        protected string retrieveset()
        {
            return CrossSecureStorage.Current.GetValue("myCookie");
        }

    }

}
