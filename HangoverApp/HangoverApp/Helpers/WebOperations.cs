using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<string> SearchByPostcode(string authLoginToken, string postcode)
        {
            var url = "https://www.just-eat.co.uk/search/do";
            var source = "";
            Task task = new Task(() =>
            {
                source = PostAction(url, authLoginToken, "", postcode).Result;
            });

            
            task.Start();
            task.Wait();

            return source;

        }

       public async Task<string> PostAction(string url, string cookie, string operation, string postcode)
        {
            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var httpClient = new HttpClient(handler))
            {
                var parameters = new Dictionary<string, string> { { "Cookie", cookie},
                    { "cuisine", "" } ,
                    {"postcode", postcode}
                };
                var encodedContent = new FormUrlEncodedContent(parameters);

                var response = await httpClient.PostAsync(url, encodedContent).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }

            return null;
        }


        public async Task<string> AccessTheWebAsync(string url, string cookie, string operation)
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
