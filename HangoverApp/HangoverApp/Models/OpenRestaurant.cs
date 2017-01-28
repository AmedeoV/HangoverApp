using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HangoverApp.Models
{
    public class OpenRestaurant
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        //[Microsoft.WindowsAzure.MobileServices.Version]
        public string Version { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Cuisine { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string RestaurantDetails { get; set; } = string.Empty;

        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string RestaurantStatus { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string StarsRating { get; set; } = string.Empty;


        [JsonIgnore]
        public Uri ImageUri
        {
            get { return new System.Uri(Image); }
        }

        [JsonIgnore]
        public Uri StarsRatingUri
        {
            get { return new System.Uri(StarsRating); }
        }

        [JsonIgnore]
        public Uri UrlUri
        {
            get { return new System.Uri(Url); }
        }

        public double Latitude { get; set; } = 0;
        public double Longitude { get; set; } = 0;

        public string MondayOpen { get; set; } = string.Empty;
        public string MondayClose { get; set; } = string.Empty;
        public string TuesdayOpen { get; set; } = string.Empty;
        public string TuesdayClose { get; set; } = string.Empty;
        public string WednesdayOpen { get; set; } = string.Empty;
        public string WednesdayClose { get; set; } = string.Empty;
        public string ThursdayOpen { get; set; } = string.Empty;
        public string ThursdayClose { get; set; } = string.Empty;
        public string FridayOpen { get; set; } = string.Empty;
        public string FridayClose { get; set; } = string.Empty;
        public string SaturdayOpen { get; set; } = string.Empty;
        public string SaturdayClose { get; set; } = string.Empty;
        public string SundayOpen { get; set; } = string.Empty;
        public string SundayClose { get; set; } = string.Empty;

        public string ReviewsNumber { get; set; } = string.Empty;
        public string LocationCode { get; set; } = string.Empty;


    }
}
