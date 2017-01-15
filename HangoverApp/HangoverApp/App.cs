using Akavache;
using HangoverApp.Helper;
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
using Xamarin.Forms;

namespace HangoverApp
{
    public class App : Application, ILoginManager
    {
        public static App Current;
        public static Color BrandColor = Color.FromHex("#FF9800");
        public App()
        {
            Current = this;
            BlobCache.ApplicationName = "CPMobile";

            //uncomment to remove the cookie from the phone
            //CrossSecureStorage.Current.DeleteKey("myCookie");
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
            new RootPage();
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

        public static bool DoBack
        {
            get
            {
                MasterDetailPage mainPage = App.Current.MainPage as RootPage;

                if (mainPage != null)
                {
                    bool canDoBack = mainPage.Detail.Navigation.NavigationStack.Count > 2 || mainPage.IsPresented;

                    // we are on a top level page and the Master menu is NOT showing
                    if (!canDoBack)
                    {
                        // don't exit the app just show the Master menu page
                        mainPage.IsPresented = true;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return true;
            }
        }
    }
}
