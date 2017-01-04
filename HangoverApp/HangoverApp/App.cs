using Akavache;
using HangoverApp.Helper;
using HangoverApp.Models;
using HangoverApp.Views;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HangoverApp
{
    public class App : Application, ILoginManager
    {
        // https://gist.github.com/ChaseFlorell/32e1f5c1187d2a7e4835
        public static App Current;
        public static Color BrandColor = Color.FromHex("#FF9800");
        public App()
        {
            // The root page of your application
            Current = this;
            BlobCache.ApplicationName = "CPMobile";
            var authLoginToken = CrossSecureStorage.Current.GetValue("myCookie");

            //try to login using post and the cookie
            bool isLoggedIn = false;

            isLoggedIn = TryToLogIn(authLoginToken);

            if (isLoggedIn == false)
            {
                MainPage = new LoginPage();
            }
            else
                MainPage = new RootPage();

        }

        private bool TryToLogIn(string authLoginToken)
        {

            throw new NotImplementedException();
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
