using Akavache;
using HangoverApp.Helpers;
using HangoverApp.Models;
using HangoverApp.Views;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.OS;
using Android.Widget;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace HangoverApp
{
    public class App : Application
    {
        public static App Current;
        public static Color BrandColor = Color.FromHex("#8BC34A");
        public App()
        {
            Current = this;
            //uncomment to remove the cookie from the phone
            //CrossSecureStorage.Current.DeleteKey("myCookie");
            //CrossSecureStorage.Current.DeleteKey("postcode");
            var authLoginToken = CrossSecureStorage.Current.GetValue("myCookie");

            WebOperations operation = new WebOperations();
            var isLoggedIn = operation.TryToLogIn(authLoginToken);

            if (isLoggedIn == false)
            {
                MainPage = new LoginPage();
            }
            else
                MainPage = new MainListPage();

        }


        #region ILoginManager implementation

        public void ShowRootPage()
        {
        }

        public void LogOut()
        {
            Properties["IsLoggedIn"] = false;
            //new LoginPage (this);
        }

        #endregion

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
