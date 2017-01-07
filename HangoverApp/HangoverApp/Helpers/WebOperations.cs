using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Plugin.SecureStorage;

namespace HangoverApp.Helpers
{
    public class WebOperations
    {
        public bool TryToLogIn(string authLoginToken)
        {

            var url = "https://www.just-eat.co.uk/";
            var myXMLstring = "";
            Task task = new Task(() =>
            {
                myXMLstring = AccessTheWebAsync(url, authLoginToken, "").Result;
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
        public bool LogOut(string authLoginToken)
        {
            CrossSecureStorage.Current.DeleteKey("myCookie");
            var url = "https://www.just-eat.co.uk/";
            var myXMLstring = "";
            Task task = new Task(() =>
            {
                myXMLstring = AccessTheWebAsync(url, authLoginToken, "/account/logout?returnurl=%2F").Result;
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

        async Task<string> AccessTheWebAsync(string url, string cookie, string operation)
        {
            var baseAddress = new Uri(url);
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                var message = new HttpRequestMessage(HttpMethod.Get, operation);
                message.Headers.Add("Cookie", cookie);
                var result = await client.SendAsync(message);
                result.EnsureSuccessStatusCode();

                return await result.Content.ReadAsStringAsync();
            }

        }
    }
}
