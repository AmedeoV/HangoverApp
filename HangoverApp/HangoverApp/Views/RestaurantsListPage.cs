using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Webkit;
using HangoverApp.Helpers;
using HtmlAgilityPack;
using Plugin.SecureStorage;
using Xamarin.Forms;
using WebView = Xamarin.Forms.WebView;

namespace HangoverApp.Views
{
    public class RestaurantsListPage : ContentPage
    {
        public RestaurantsListPage(string source)
        {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(source);
            var activityIndicator = new ProgressBar()
            {
                Progress = .2,
                IsVisible = false
            };


            Button btnChangePostcode = new Button()
            {
                Text = "Change Postcode",
                BackgroundColor = Color.FromHex("#8BC34A")
            };

            Xamarin.Forms.WebView objWebView1 = new Xamarin.Forms.WebView();
            StackLayout objStackLayout = new StackLayout()
            {
            };
            objWebView1.HeightRequest = 300;
            objStackLayout.Children.Add(objWebView1);

            objStackLayout.Children.Add(btnChangePostcode);
            objStackLayout.Children.Add(activityIndicator);


            btnChangePostcode.Clicked +=
                 (sender, e) =>
                {
                    ChangePostcodeEvent(sender, e, activityIndicator, objWebView1);
                };

            HtmlNode node = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "c-serp__open o-card c-serp__list");

            string newHtml = node.InnerHtml;

            var scriptsAndStyle = document.DocumentNode.Descendants()
                       .Where(n => n.Name == "script").ToList();

            foreach (var item in scriptsAndStyle)
            {
                newHtml = newHtml + item.OuterHtml;
            }

            var baseUri = new Uri("https://www.just-eat.co.uk");
            var pattern = @"(?<name>src|href|data-original)=""(?<value>/[^""]*)""";
            var matchEvaluator = new MatchEvaluator(
                match =>
                {
                    var value = match.Groups["value"].Value;
                    Uri uri;

                    if (Uri.TryCreate(baseUri, value, out uri))
                    {
                        var name = match.Groups["name"].Value;
                        return string.Format("{0}=\"{1}\"", name, uri.AbsoluteUri);
                    }
                    return null;
                });
            var adjustedHtml = Regex.Replace(newHtml, pattern, matchEvaluator);

            adjustedHtml = adjustedHtml + @"<link href='file:///android_asset/globalCss.css' rel='stylesheet'/>" + @"<link href='file:///android_asset/restaurantCss.css' rel='stylesheet'/>" + @"<link href='file:///android_asset/fontCss.css' rel='stylesheet'/>";

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = adjustedHtml;
            objWebView1.Source = htmlSource;

            objWebView1.VerticalOptions = LayoutOptions.FillAndExpand;
            objWebView1.HorizontalOptions = LayoutOptions.FillAndExpand;
            objWebView1.Navigating += async (sender, e) =>
            {
                await ShowRestaurantMenu(e, activityIndicator, objWebView1);
            };

            //
            //
            this.Content = objStackLayout;

        }

        async void ChangePostcodeEvent(object sender, EventArgs ea, ProgressBar activityIndicator, WebView objWebView1)
        {
            activityIndicator.IsVisible = true;
            await activityIndicator.ProgressTo(1, 950, Easing.Linear);
            objWebView1.IsEnabled = false;

            Page page = new Page();
            page.Navigation.PushModalAsync(new MainListPage());
            if (Device.OS == TargetPlatform.Android)
                Application.Current.MainPage = new MainListPage();
        }

        private async Task ShowRestaurantMenu(WebNavigatingEventArgs e, ProgressBar activityIndicator, WebView objWebView1)
        {
            e.Cancel = true;
            objWebView1.IsEnabled = false;
            IsBusy = true;
            activityIndicator.IsVisible = true;
            await activityIndicator.ProgressTo(1, 950, Easing.Linear);

            if (e.Url.Contains("menu"))
            {
                Page page = new Page();
                WebOperations operation = new WebOperations();

                var cookie = CookieManager.Instance.GetCookie(e.Url);
                CrossSecureStorage.Current.SetValue("myCookie", cookie);
                var source = await operation.AccessTheWebAsync(e.Url, cookie, "");

                await page.Navigation.PushModalAsync(new RestaurantMenuPage(source));
                if (Device.OS == TargetPlatform.Android)
                    Application.Current.MainPage = new RestaurantMenuPage(source);

            }
        }
    }
}
