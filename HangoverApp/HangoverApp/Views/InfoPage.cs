using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Webkit;
using HangoverApp.Helpers;
using HtmlAgilityPack;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class InfoPage : ContentPage
    {
        private readonly NavigationHelper _navigationHelper;
        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }
        public InfoPage(string source)
        {
            Button btnCheckOtherRestaurants = new Button()
            {
                Text = "Check other restaurants!",
                BackgroundColor = Color.FromHex("#8BC34A")
            };

            var activityIndicator = new ProgressBar()
            {
                Progress = .2,
                IsVisible = false
            };

            StackLayout objStackLayout = new StackLayout()
            {
            };

            BackgroundImage = "RestaurantsBackground.png";
            Xamarin.Forms.WebView objWebView1 = new Xamarin.Forms.WebView();
            objWebView1.HeightRequest = 300;
            objStackLayout.Children.Add(objWebView1);
            objStackLayout.Children.Add(btnCheckOtherRestaurants);
            objStackLayout.Children.Add(activityIndicator);

            //
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(source);

            HtmlNode node = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "restaurantParts");
            HtmlNode removeNode = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "restaurantMap o-card o-card--padding");
            node.RemoveChild(removeNode);

            var scriptsAndStyle = document.DocumentNode.Descendants()
           .Where(n => n.Name == "script" | n.Name == "noscript" | n.Name == "link").ToList();

            string newHtml = node.InnerHtml;

            foreach (var item in scriptsAndStyle)
            {
                newHtml = newHtml + item.OuterHtml;
            }

            var baseUri = new Uri("https://www.just-eat.co.uk/");
            var pattern = @"(?<name>src|href|action)=""(?<value>/[^""]*)""";
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

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = adjustedHtml;
            objWebView1.Source = htmlSource;

            //objWebView1.Source = objUrlToNavigateTo;

            objWebView1.VerticalOptions = LayoutOptions.FillAndExpand;
            objWebView1.HorizontalOptions = LayoutOptions.FillAndExpand;
            _navigationHelper = new NavigationHelper();
            objWebView1.Navigating +=
                (sender, e) =>
                {
                    Views.NavigationHelper.ConfirmNavigationAndSendToWebBrowserApp(sender, e, activityIndicator);
                };
            //
            //
            this.Content = objStackLayout;
            btnCheckOtherRestaurants.Clicked +=
                (sender, e) =>
                {
                    NavigationHelper.CheckOtherRestaurantsEvent(sender, e, activityIndicator, objWebView1);
                };

        }


    }
}
