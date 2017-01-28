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

namespace HangoverApp.Views
{
    public class RestaurantMenuPage : ContentPage
    {
        private readonly NavigationHelper _navigationHelper;
        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }
        public RestaurantMenuPage(string source)
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
            HtmlNode removeNode = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value == "postCodeAdvisory");

            HtmlNode node = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "restaurantParts");
            node.RemoveChild(removeNode);

            HtmlNode basketNode = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value == "mobileFixedBasket");
            HtmlNode anotherBasketNode = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value == "basket");
            HtmlNode userNotification = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("id") && d.Attributes["id"].Value == "userNotification");


            var scriptsAndStyle = document.DocumentNode.Descendants()
           .Where(n => n.Name == "script" | n.Name == "noscript" | n.Name == "link").ToList();

            //string newHtml = node.InnerHtml;


            string newHtml = node.InnerHtml + anotherBasketNode.OuterHtml + basketNode.OuterHtml + userNotification.OuterHtml + @"<div class='advisoryDialog hide' id='postCodeAdvisory' role='dialog' aria-labelledby='locationCheckPrompt' aria-describedby='advisoryDescription' tabindex='1'>
                <div class='advisoryContent'>
                    <div id='postcodePromptContainer'>
<div class='LocationPrompt' id='postcodeFormContainer'>
    <h2 id='locationCheckPrompt'>Great choice!</h2>
        <p id='advisoryDescription'>Are you happy? - Just click confirm...</p>
    <form action='https://www.just-eat.co.uk/restaurants-china-palace-london-wandsworth/menu/postcode?restaurantId=51069&amp;menuId=178222&amp;isCustomisable=False&amp;productId=0' id='postcodeForm' method='post'>        <div id='errorSummaryLink' class='hidden'></div>
        <div class='standardControl'>
            <div class='control'>
                <input autocomplete='on' data-val='true' data-val-regex='This doesn&#39;t look like a UK postcode, can you enter it again please?' data-val-regex-pattern='^\s*[A-z]{1,2}[ ]?[0-9]{1,2}[A-z]?[ ]*[0-9][A-z]{2}\s*$' data-val-required='Please enter your full UK postcode' id='postcodeEntry' name='Postcode' placeholder='E.g. SW11 1BD' tabindex='2' type='hidden' value='" + CrossSecureStorage.Current.GetValue("postcode") + @"' disabled />
            </div>

            <div class='actions'>
                <button type='submit' class='submit o-btn o-btn--primary' tabindex='2'>Confirm</button>
            </div>

        </div>
</form></div>

<div class='LocationPrompt' id='invalidPostcode'>
    <h2 id='locationCheckInvalid'>China Palace London restaurant menu in London – Order from Just Eat</h2>
    <div class='standardControl'>
        <div class='actions'>
            <a class='moreRestaurants o-btn o-btn--primary' role='button' tabindex='2'>More restaurants nearby</a>
        </div>
        <div class='actions'>
            <a href='/restaurants-china-palace-london-wandsworth/menu/postcode?menuId=178222&amp;restaurantId=51069&amp;productId=0&amp;isCustomisable=False' class='enterPostcode o-btn o-btn--secondary' role='button' tabindex='3'>Enter new postcode</a>
        </div>
    </div>
</div>

<div class='LocationPrompt' id='validPostcode'>
    <h2 id='locationCheckValid'>Great! Continue ordering</h2>
    <p>
        <img alt='Sucessfully submited' src='https://dy3erx8o0a6nh.cloudfront.net/images/star-normal.png' />
    </p>
<form action='/restaurants-china-palace-london-wandsworth/51069/menus/178222/basket/items/add?isCustomisable=False&amp;productId=0' method='post'>        <div class='actions'>
            <button type='submit' class='submit o-btn o-btn--primary' tabindex='2'>Continue</button>
        </div>
</form></div>


                    </div>
                </div>
            </div>" + @"<link href='file:///android_asset/globalCss.css' rel='stylesheet'/>" +
                           @"<link href='file:///android_asset/restaurantCss.css' rel='stylesheet'/>" +
                           @"<link href='file:///android_asset/fontCss.css' rel='stylesheet'/>";

            foreach (var item in scriptsAndStyle)
            {
                newHtml = newHtml + item.OuterHtml;
            }

            var baseUri = new Uri("https://www.just-eat.co.uk");
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

            //adjustedHtml = adjustedHtml + @"<link href='file:///android_asset/globalCss.css' rel='stylesheet'/>" +
            //               @"<link href='file:///android_asset/restaurantCss.css' rel='stylesheet'/>" +
            //               @"<link href='file:///android_asset/fontCss.css' rel='stylesheet'/>";

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = adjustedHtml;
            objWebView1.Source = htmlSource;

            //objWebView1.Source = objUrlToNavigateTo;

            objWebView1.VerticalOptions = LayoutOptions.FillAndExpand;
            objWebView1.HorizontalOptions = LayoutOptions.FillAndExpand;
            _navigationHelper = new NavigationHelper(this);
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
