using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HangoverApp.Helpers;
using HtmlAgilityPack;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class SearchPage : ContentPage
    {

        public SearchPage(string source)
        {
            Button btnCheckOtherRestaurants = new Button()
            {
                Text = "Check other restaurants!",
                BackgroundColor = Color.FromHex("#8BC34A")
            };


            StackLayout objStackLayout = new StackLayout()
            {
            };
            //
            Xamarin.Forms.WebView objWebView1 = new Xamarin.Forms.WebView();
            objWebView1.HeightRequest = 300;

            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(source);

            var scriptsAndStyle = document.DocumentNode.Descendants()
           .Where(n => n.Name == "script" |n.Name == "link").ToList();

            HtmlNode node = document.DocumentNode.Descendants("section").FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value == "search-section");


            string newHtml = node.InnerHtml;

            foreach (var item in scriptsAndStyle)
            {
                source = source + item.OuterHtml;
            }

            var baseUri = new Uri(CrossSecureStorage.Current.GetValue("myCountry"));
            var pattern = @"(?<name>src|href|action|srcset)=""(?<value>/[^""]*)""";
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
            var adjustedHtml = Regex.Replace(source, pattern, matchEvaluator);
            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = adjustedHtml;
            objWebView1.Source = htmlSource;
            objWebView1.VerticalOptions = LayoutOptions.FillAndExpand;
            objWebView1.HorizontalOptions = LayoutOptions.FillAndExpand;

            objStackLayout.Children.Add(objWebView1);
            objStackLayout.Children.Add(btnCheckOtherRestaurants);



            this.Content = objStackLayout;
        }
    }
}
