using Akavache;
using HangoverApp.Helper;
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
        // https://gist.github.com/ChaseFlorell/32e1f5c1187d2a7e4835
        public static App Current;
        public static Color BrandColor = Color.FromHex("#FF9800");
        public App()
        {
            Current = this;
            BlobCache.ApplicationName = "CPMobile";

            //uncomment to remove the cookie from the phone
             //CrossSecureStorage.Current.DeleteKey("myCookie");
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

            var url = "https://www.just-eat.co.uk/";
            var myXMLstring = "";
            Task task = new Task(() =>
            {
                myXMLstring = AccessTheWebAsync(url, authLoginToken).Result;
            });
            task.Start();
            task.Wait();

            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(myXMLstring);

            var isLoggedIn = document.DocumentNode.Descendants("div").FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "nav-container is-logged-in");

            if (isLoggedIn != null)
            {
                return true;
            }
            else
                return false;
        }

        async Task<string> AccessTheWebAsync(string url, string cookie)
        {
            var baseAddress = new Uri(url);
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var message = new HttpRequestMessage(HttpMethod.Get, "");
                message.Headers.Add("Cookie", cookie);
                var result = await client.SendAsync(message);
                result.EnsureSuccessStatusCode();

                return await result.Content.ReadAsStringAsync();
            }

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
