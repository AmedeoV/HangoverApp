using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Provider;
using HangoverApp.Models;
using HtmlAgilityPack;

namespace HangoverApp.Helpers
{
    public class RestaurantsDeserializer
    {


        public List<Restaurant> Restaurants(string source)
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(source);

            HtmlNode takingPreordersNode = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "c-serp__open o-card c-serp__list");

            var takingOrdersUrl = (from x in takingPreordersNode.DescendantNodes()
                                 where x.Name == "a" && x.Attributes.Contains("data-gtm")
                                 where x.Attributes["data-gtm"].Value != null
                                 select x.Attributes["href"].Value).ToArray();

           

            var takingOrdersRestaurantNames = (from x in takingPreordersNode.DescendantNodes()
                                   where x.Name == "h2" && x.Attributes.Contains("itemprop")
                                   where x.Attributes["itemprop"].Value == "name"
                                   select WebUtility.HtmlDecode(x.InnerText)).ToArray();

            var takingOrdersRestaurantCusine = (from x in takingPreordersNode.DescendantNodes()
                                    where x.Name == "p" && x.Attributes.Contains("itemprop")
                                    where x.Attributes["itemprop"].Value == "servesCuisine"
                                    select WebUtility.HtmlDecode(x.InnerText)).ToArray();

            var takingOrddersRestaurantLogo = (from x in takingPreordersNode.DescendantNodes()
                                  where x.Name == "img" && x.Attributes.Contains("class")
                                  where x.Attributes["class"].Value == "c-restaurant__logo js-lazy"
                                  select x.Attributes["data-original"].Value).ToArray();

            var takingOrdersRestaurantStarsImage = (from x in takingPreordersNode.DescendantNodes()
                                        where x.Name == "img" && x.Attributes.Contains("alt")
                                        where x.Attributes["alt"].Value.Contains("stars")
                                        select x.Attributes["src"].Value).ToArray();

            var takingOrdersRestaurantReviews = (from x in takingPreordersNode.DescendantNodes()
                                     where x.Name == "meta" && x.Attributes.Contains("itemprop")
                                     where x.Attributes["itemprop"].Value == "ratingCount"
                                     select x.Attributes["content"].Value).ToArray();

            //var restaurantDelivery = (from x in startNode.DescendantNodes()
            //                          where x.Name == "p" && x.Attributes.Contains("class")
            //                          where (x.Attributes["class"].Value == "c-restaurant__delivery" || x.Attributes["class"].Value == "c-restaurant__collection")
            //                          select x.InnerText).ToArray();

            //var restaurantMinimumSpend = (from x in startNode.DescendantNodes()
            //where x.Name == "p" && x.Attributes.Contains("class")
            //where (x.Attributes["class"].Value == "c-restaurant__minimum" || x.Attributes["class"].Value == "c-restaurant__delivery-start")
            //select x.InnerText).ToArray();

            var takingOrdersRestauranDetails = (from x in takingPreordersNode.DescendantNodes()
                                    where x.Name == "div" && x.Attributes.Contains("class")
                                    where (x.Attributes["class"].Value == "c-restaurant__details")
                                    select x.InnerHtml).ToList();



            List<string> restMinSpend = new List<string>();

            foreach (var x in takingOrdersRestauranDetails)
            {
                var dc = new HtmlAgilityPack.HtmlDocument();
                dc.LoadHtml(x);
                HtmlNode startNode2 = dc.DocumentNode;
                string result = Regex.Replace(startNode2.InnerText.Trim(), @"[^\S\r\n]+", " ");
                result = result.Replace(System.Environment.NewLine, ", ");
                result = WebUtility.HtmlDecode(result);
                restMinSpend.Add(result);
                  
                }

            var restaurantDeta = restMinSpend.ToArray();

            HtmlNode notTakingPreodersNode = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "c-serp__closed o-card c-serp__list");

            var notTakingOrdersUrl = (from x in notTakingPreodersNode.DescendantNodes()
                                   where x.Name == "a" && x.Attributes.Contains("data-gtm")
                                   where x.Attributes["data-gtm"].Value != null
                                   select x.Attributes["href"].Value).ToArray();



            var notTakingOrdersRestaurantNames = (from x in notTakingPreodersNode.DescendantNodes()
                                               where x.Name == "h2" && x.Attributes.Contains("itemprop")
                                               where x.Attributes["itemprop"].Value == "name"
                                               select WebUtility.HtmlDecode(x.InnerText)).ToArray();

            var notTakingOrdersRestaurantCusine = (from x in notTakingPreodersNode.DescendantNodes()
                                                where x.Name == "p" && x.Attributes.Contains("itemprop")
                                                where x.Attributes["itemprop"].Value == "servesCuisine"
                                                select WebUtility.HtmlDecode(x.InnerText)).ToArray();

            var notTakingOrddersRestaurantLogo = (from x in notTakingPreodersNode.DescendantNodes()
                                               where x.Name == "img" && x.Attributes.Contains("class")
                                               where x.Attributes["class"].Value == "c-restaurant__logo js-lazy"
                                               select x.Attributes["data-original"].Value).ToArray();

            var notTakingOrdersRestaurantStarsImage = (from x in notTakingPreodersNode.DescendantNodes()
                                                    where x.Name == "img" && x.Attributes.Contains("alt")
                                                    where x.Attributes["alt"].Value.Contains("stars")
                                                    select x.Attributes["src"].Value).ToArray();

            var notTakingOrdersRestaurantReviews = (from x in notTakingPreodersNode.DescendantNodes()
                                                 where x.Name == "meta" && x.Attributes.Contains("itemprop")
                                                 where x.Attributes["itemprop"].Value == "ratingCount"
                                                 select x.Attributes["content"].Value).ToArray();

            var notTakingOrdersRestauranDetails = (from x in notTakingPreodersNode.DescendantNodes()
                                                where x.Name == "div" && x.Attributes.Contains("class")
                                                where (x.Attributes["class"].Value == "c-restaurant__details")
                                                select x.InnerHtml).ToList();



            List<string> notRestMinSpend = new List<string>();

            foreach (var x in notTakingOrdersRestauranDetails)
            {
                var dc = new HtmlAgilityPack.HtmlDocument();
                dc.LoadHtml(x);
                HtmlNode startNode2 = dc.DocumentNode;
                string result = Regex.Replace(startNode2.InnerText.Trim(), @"[^\S\r\n]+", " ");
                result = result.Replace(System.Environment.NewLine, ", ");
                result = WebUtility.HtmlDecode(result);
                notRestMinSpend.Add(result);

            }

            var notRestaurantDeta = notRestMinSpend.ToArray();

            for (int i = 0; i < takingOrdersRestaurantNames.Count(); i++)
            {
                restaurants.Add(new Restaurant()
                {
                    Name = takingOrdersRestaurantNames[i],
                    Cuisine = takingOrdersRestaurantCusine[i],
                    Logo = takingOrddersRestaurantLogo[i],
                    StarsImage = takingOrdersRestaurantStarsImage[i],
                    Reviews = takingOrdersRestaurantReviews[i],
                    Url = takingOrdersUrl[i],
                    RestaurantDetails = restaurantDeta[i],
                    IsPreorder = false
                    //MinimumSpend = restaurantMinimumSpend[i]
                });

            }

            for (int i = 0; i < notTakingOrdersRestaurantNames.Count(); i++)
            {
                restaurants.Add(new Restaurant()
                {
                    Name = notTakingOrdersRestaurantNames[i],
                    Cuisine = notTakingOrdersRestaurantCusine[i],
                    Logo = notTakingOrddersRestaurantLogo[i],
                    StarsImage = notTakingOrdersRestaurantStarsImage[i],
                    Reviews = notTakingOrdersRestaurantReviews[i],
                    Url = notTakingOrdersUrl[i],
                    RestaurantDetails = notRestaurantDeta[i],
                    IsPreorder = true
                    //MinimumSpend = restaurantMinimumSpend[i]
                });

            }
            return restaurants;
        }


    }
}
