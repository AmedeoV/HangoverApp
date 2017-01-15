using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            HtmlNode startNode = document.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "c-serp__open o-card c-serp__list");

            var restauranUrls = (from x in startNode.DescendantNodes()
                                 where x.Name == "a" && x.Attributes.Contains("data-gtm")
                                 where x.Attributes["data-gtm"].Value != null
                                 select x.Attributes["href"].Value).ToArray();

            var restaurantNames = (from x in startNode.DescendantNodes()
                                where x.Name == "h2" && x.Attributes.Contains("itemprop")
                                where x.Attributes["itemprop"].Value == "name"
                                select x.InnerText).ToArray();

            var restaurantCusine = (from x in startNode.DescendantNodes()
                                   where x.Name == "p" && x.Attributes.Contains("itemprop")
                                   where x.Attributes["itemprop"].Value == "servesCuisine"
                                    select x.InnerText).ToArray();

            var restaurantLogo = (from x in startNode.DescendantNodes()
                                    where x.Name == "img" && x.Attributes.Contains("class")
                                    where x.Attributes["class"].Value == "c-restaurant__logo js-lazy"
                                  select x.Attributes["data-original"].Value).ToArray();

            var restaurantStarsImage = (from x in startNode.DescendantNodes()
                                  where x.Name == "img" && x.Attributes.Contains("alt")
                                  where x.Attributes["alt"].Value.Contains("stars")
                                  select x.Attributes["src"].Value).ToArray();

            var restaurantReviews = (from x in startNode.DescendantNodes()
                                        where x.Name == "meta" && x.Attributes.Contains("itemprop")
                                        where x.Attributes["itemprop"].Value == "ratingCount"
                                     select x.Attributes["content"].Value).ToArray();

            //var restaurantDistance = (from x in startNode.DescendantNodes()
            //                       where x.Name == "p" && x.Attributes.Contains("class")
            //                       where x.Attributes["class"].Value == "c-restaurant__distance"
            //                          select x.InnerText).ToArray();

            for (int i = 0; i < restaurantNames.Count(); i++)
            {
                restaurants.Add(new Restaurant()
                {
                    Name = restaurantNames[i],
                    Cuisine = restaurantCusine[i],
                    Logo = restaurantLogo[i],
                    StarsImage = restaurantStarsImage[i],
                    Reviews = restaurantReviews[i],
                    Url = restauranUrls[i]
                });

            }
            return restaurants;
        } 


    }
}
