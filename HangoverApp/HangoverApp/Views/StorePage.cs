using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HangoverApp.Models;
using HangoverApp.ViewModels;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class StorePage : ContentPage
    {
        OpenRestaurant OpenRestaurant { get; set; }
        bool isNew;
        private EntryCell locationCode;

        EntryCell mondayOpen, mondayClose, tuesdayOpen, tuesdayClose, wednesdayOpen, wednesdayClose,
        thursdayOpen, thursdayClose, fridayOpen, fridayClose, saturdayOpen, saturdayClose, sundayOpen, sundayClose,
        reviewsNumber, streetAddress, city, state, zipCode, restaurantStatus, name, cuisine, imageUrl, starsRatingUrl, restaurantDetails, restaurantUrl;

        TextCell latitude, longitude, detectLatLong, refreshImage;
        Image image;
        readonly IDataStore dataStore;
        public StorePage(OpenRestaurant openRestaurant)
        {

            dataStore = DependencyService.Get<IDataStore>();
            OpenRestaurant = openRestaurant;
            if (OpenRestaurant == null)
            {
                OpenRestaurant = new OpenRestaurant();
                OpenRestaurant.MondayOpen = "9am";
                OpenRestaurant.TuesdayOpen = "9am";
                OpenRestaurant.WednesdayOpen = "9am";
                OpenRestaurant.ThursdayOpen = "9am";
                OpenRestaurant.FridayOpen = "9am";
                OpenRestaurant.SaturdayOpen = "9am";
                OpenRestaurant.SundayOpen = "12pm";
                OpenRestaurant.MondayClose = "8pm";
                OpenRestaurant.TuesdayClose = "8pm";
                OpenRestaurant.WednesdayClose = "8pm";
                OpenRestaurant.ThursdayClose = "8pm";
                OpenRestaurant.FridayClose = "8pm";
                OpenRestaurant.SaturdayClose = "8pm";
                OpenRestaurant.SundayClose = "6pm";
                isNew = true;
            }

            Title = isNew ? "New OpenRestaurant" : "Edit OpenRestaurant";

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Save",
                Command = new Command(async (obj) =>
                {
                    OpenRestaurant.Name = name.Text.Trim();
                    OpenRestaurant.Cuisine = cuisine.Text.Trim();
                    OpenRestaurant.Url = restaurantUrl.Text.Trim();
                    OpenRestaurant.RestaurantDetails = restaurantDetails.Text.Trim();
                    OpenRestaurant.City = city.Text.Trim();
                    OpenRestaurant.ReviewsNumber = reviewsNumber.Text.Trim();
                    OpenRestaurant.Image = imageUrl.Text.Trim();
                    OpenRestaurant.StarsRating = starsRatingUrl.Text.Trim();
                    OpenRestaurant.StreetAddress = streetAddress.Text.Trim();
                    OpenRestaurant.State = state.Text.Trim();
                    OpenRestaurant.ZipCode = zipCode.Text.Trim();
                    OpenRestaurant.LocationCode = locationCode.Text.Trim();
                    OpenRestaurant.RestaurantStatus = restaurantStatus.Text.Trim();
                    double lat;
                    double lng;

                    var parse1 = double.TryParse(latitude.Text.Trim(), out lat);
                    var parse2 = double.TryParse(longitude.Text.Trim(), out lng);
                    OpenRestaurant.Longitude = lng;
                    OpenRestaurant.Latitude = lat;
                    OpenRestaurant.MondayOpen = mondayOpen.Text.Trim();
                    OpenRestaurant.MondayClose = mondayClose.Text.Trim();
                    OpenRestaurant.TuesdayOpen = tuesdayOpen.Text.Trim();
                    OpenRestaurant.TuesdayClose = tuesdayClose.Text.Trim();
                    OpenRestaurant.WednesdayOpen = wednesdayOpen.Text.Trim();
                    OpenRestaurant.WednesdayClose = wednesdayClose.Text.Trim();
                    OpenRestaurant.ThursdayOpen = thursdayOpen.Text.Trim();
                    OpenRestaurant.ThursdayClose = thursdayClose.Text.Trim();
                    OpenRestaurant.FridayOpen = fridayOpen.Text.Trim();
                    OpenRestaurant.FridayClose = fridayClose.Text.Trim();
                    OpenRestaurant.SaturdayOpen = saturdayOpen.Text.Trim();
                    OpenRestaurant.SaturdayClose = saturdayClose.Text.Trim();
                    OpenRestaurant.SundayOpen = sundayOpen.Text.Trim();
                    OpenRestaurant.SundayClose = sundayClose.Text.Trim();




                    bool isAnyPropEmpty = OpenRestaurant.GetType().GetTypeInfo().DeclaredProperties
                        .Where(p => p.GetValue(OpenRestaurant) is string && p.CanRead && p.CanWrite && p.Name != "State") // selecting only string props
                        .Any(p => string.IsNullOrWhiteSpace((p.GetValue(OpenRestaurant) as string)));

                    if (isAnyPropEmpty || !parse1 || !parse2)
                    {
                        await DisplayAlert("Not Valid", "Some fields are not valid, please check", "OK");
                        return;
                    }
                    Title = "SAVING...";
                    if (isNew)
                    {
                        await dataStore.AddStoreAsync(OpenRestaurant);
                    }
                    else
                    {
                        await dataStore.UpdateStoreAsync(OpenRestaurant);
                    }

                    await DisplayAlert("Saved", "Please refresh OpenRestaurant list", "OK");
                    await Navigation.PopAsync();
                })
            });


            Content = new TableView
            {
                HasUnevenRows = true,
                Intent = TableIntent.Form,
                Root = new TableRoot {
                    new TableSection ("Information") {
                        (name = new EntryCell {Label = "Name", Text = OpenRestaurant.Name}),
                        (cuisine = new EntryCell {Label = "Location Hint", Text = OpenRestaurant.Cuisine}),
                        (restaurantDetails = new EntryCell {Label = "Restaurant Details", Text = OpenRestaurant.RestaurantDetails}),
                        (reviewsNumber = new EntryCell {Label = "Phone Number", Text = OpenRestaurant.ReviewsNumber, Placeholder ="555-555-5555"}),
                        (locationCode = new EntryCell {Label = "Location Code", Text = OpenRestaurant.LocationCode}),

                    },
                    new TableSection ("Image") {
                        (imageUrl = new EntryCell { Label="Image URL", Text = OpenRestaurant.Image, Placeholder = ".png or .jpg image link" }),
                        (refreshImage = new TextCell()
                            {
                                Text="Refresh Image"
                            }),
                        new ViewCell { View = (image = new Image
                            {
                                HeightRequest = 400,
                                VerticalOptions = LayoutOptions.FillAndExpand
                            })
                        }
                    },
                    new TableSection ("StarsRating") {
                        (imageUrl = new EntryCell { Label="StarsRating URL", Text = OpenRestaurant.StarsRating, Placeholder = ".png or .jpg image link" }),
                        (refreshImage = new TextCell()
                            {
                                Text="Refresh Image"
                            }),
                        new ViewCell { View = (image = new Image
                            {
                                HeightRequest = 10,
                                VerticalOptions = LayoutOptions.FillAndExpand
                            })
                        }
                    },
                    new TableSection ("Address") {
                        (streetAddress = new EntryCell {Label = "Street Address", Text = OpenRestaurant.StreetAddress }),
                        (city = new EntryCell {Label = "City", Text = OpenRestaurant.City }),
                        (state = new EntryCell {Label = "State", Text = OpenRestaurant.State }),
                        (zipCode = new EntryCell {Label = "Zipcode", Text = OpenRestaurant.ZipCode }),
                        (restaurantStatus = new EntryCell{Label="RestaurantStatus", Text = OpenRestaurant.RestaurantStatus}),
                        (detectLatLong = new TextCell()
                            {
                                Text="Detect Lat/Long"
                            }),
                        (latitude = new TextCell {Text = OpenRestaurant.Latitude.ToString() }),
                        (longitude = new TextCell {Text = OpenRestaurant.Longitude.ToString() }),
                    },


                    new TableSection ("Hours") {
                        (mondayOpen = new EntryCell {Label = "Monday Open", Text = OpenRestaurant.MondayOpen}),
                        (mondayClose = new EntryCell {Label = "Monday Close", Text = OpenRestaurant.MondayClose}),
                        (tuesdayOpen = new EntryCell {Label = "Tuesday Open", Text = OpenRestaurant.TuesdayOpen}),
                        (tuesdayClose = new EntryCell {Label = "Tuesday Close", Text = OpenRestaurant.TuesdayClose}),
                        (wednesdayOpen = new EntryCell {Label = "Wedneday Open", Text = OpenRestaurant.WednesdayOpen}),
                        (wednesdayClose = new EntryCell {Label = "Wedneday Close", Text = OpenRestaurant.WednesdayClose}),
                        (thursdayOpen = new EntryCell {Label = "Thursday Open", Text = OpenRestaurant.ThursdayOpen}),
                        (thursdayClose = new EntryCell {Label = "Thursday Close", Text = OpenRestaurant.ThursdayClose}),
                        (fridayOpen = new EntryCell {Label = "Friday Open", Text = OpenRestaurant.FridayOpen}),
                        (fridayClose = new EntryCell {Label = "Friday Close", Text = OpenRestaurant.FridayClose}),
                        (saturdayOpen = new EntryCell {Label = "Saturday Open", Text = OpenRestaurant.SaturdayOpen}),
                        (saturdayClose =new EntryCell {Label = "Saturday Close", Text = OpenRestaurant.SaturdayClose}),
                        (sundayOpen = new EntryCell {Label = "Sunday Open", Text = OpenRestaurant.SundayOpen}),
                        (sundayClose = new EntryCell {Label = "Sunday Close", Text = OpenRestaurant.SundayClose}),
                    },
                },
            };

            refreshImage.Tapped += (sender, e) =>
            {
                image.Source = ImageSource.FromUri(new Uri(imageUrl.Text));
            };

            //detectLatLong.Tapped += async (sender, e) =>
            //{
            //    var coder = new Xamarin.Forms.Maps.Geocoder();
            //    var oldTitle = Title;
            //    Title = "Please wait...";
            //    var locations = await coder.GetPositionsForAddressAsync(streetAddress.Text + " " + city.Text + ", " + state.Text + " " + zipCode.Text + " " + restaurantStatus.Text);
            //    Title = oldTitle;
            //    foreach (var location in locations)
            //    {
            //        latitude.Text = location.Latitude.ToString();
            //        longitude.Text = location.Longitude.ToString();
            //        break;
            //    }
            //};

            SetBinding(Page.IsBusyProperty, new Binding("IsBusy"));
        }
    }
}
