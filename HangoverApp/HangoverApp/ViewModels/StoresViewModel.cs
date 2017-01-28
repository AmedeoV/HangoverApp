using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using HangoverApp.Helpers;
using HangoverApp.Models;
using HangoverApp.Views;
using MvvmHelpers;

namespace HangoverApp.ViewModels
{
    public class StoresViewModel : ViewModelBase
    {
        readonly IDataStore dataStore;
        public ObservableRangeCollection<OpenRestaurant> Stores { get; set; }
        public ObservableRangeCollection<Grouping<string, OpenRestaurant>> StoresGrouped { get; set; }
        public bool ForceSync { get; set; }
        public StoresViewModel(Page page) : base(page)
        {
            dataStore = DependencyService.Get<IDataStore>();
            Stores = new ObservableRangeCollection<OpenRestaurant>();
            StoresGrouped = new ObservableRangeCollection<Grouping<string, OpenRestaurant>>();
        }
        public Action<OpenRestaurant> ItemSelected { get; set; }

        OpenRestaurant _selectedOpenRestaurant;
        public OpenRestaurant SelectedOpenRestaurant
        {
            get { return _selectedOpenRestaurant; }
            set
            {
                _selectedOpenRestaurant = value;
                OnPropertyChanged("_selectedOpenRestaurant");
                if (_selectedOpenRestaurant == null)
                    return;

                if (ItemSelected == null)
                {
                    page.Navigation.PushAsync(new StorePage(_selectedOpenRestaurant));
                    SelectedOpenRestaurant = null;
                }
                else
                {
                    ItemSelected.Invoke(_selectedOpenRestaurant);
                }
            }
        }


        public async Task DeleteStore(OpenRestaurant openRestaurant)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                await dataStore.RemoveStoreAsync(openRestaurant);
                Stores.Remove(openRestaurant);
                Sort();
            }
            catch (Exception ex)
            {
                await page.DisplayAlert("Uh Oh :(", $"Unable to remove {openRestaurant?.Name ?? "Unknown"}, please try again: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }



        }

        private Command getStoresCommand;
        public async void GetStoresCommand(List<Restaurant> restaurants)
        {
            await ExecuteGetStoresCommand(restaurants);
        }

        private async Task ExecuteGetStoresCommand(List<Restaurant> restaurants)
        {
            if (IsBusy)
                return;

            if (ForceSync)
                Settings.LastSync = DateTime.Now.AddDays(-30);

            IsBusy = true;
            var showAlert = false;
            try
            {
                Stores.Clear();

                //var stores = await dataStore.GetStoresAsync();
                var openRestaurants = new List<OpenRestaurant>();

                foreach (var restaurant in restaurants)
                {
                    if (!restaurant.IsPreorder)
                    {
                        openRestaurants.Add(new OpenRestaurant()
                        {
                            RestaurantStatus = "Open Restaurants",
                            Name = restaurant.Name,
                            Image = "https:" + restaurant.Logo,
                            ReviewsNumber = "(" + restaurant.Reviews + ")",
                            StarsRating = "https:" + restaurant.StarsImage,
                            Cuisine = restaurant.Cuisine,
                            RestaurantDetails = restaurant.RestaurantDetails,
                            Url = restaurant.Url
                        });
                    }
                    else
                    {
                        openRestaurants.Add(new OpenRestaurant()
                        {
                            RestaurantStatus = "Pre-orders",
                            Name = restaurant.Name,
                            Image = "https:" + restaurant.Logo,
                            ReviewsNumber = "(" + restaurant.Reviews + ")",
                            StarsRating = "https:" + restaurant.StarsImage,
                            Cuisine = restaurant.Cuisine,
                            RestaurantDetails = restaurant.RestaurantDetails,
                            Url = restaurant.Url
                        });

                    }


                }

                Stores.ReplaceRange(openRestaurants);

                Sort();
            }
            catch (Exception ex)
            {
                showAlert = true;

            }
            finally
            {
                IsBusy = false;
                //GetStoresCommand.ChangeCanExecute();
            }

            if (showAlert)
                await page.DisplayAlert("Uh Oh :(", "Unable to gather stores.", "OK");


        }

        private void Sort()
        {

            StoresGrouped.Clear();
            var sorted = from store in Stores
                         orderby store.RestaurantStatus, store.City
                         group store by store.RestaurantStatus into storeGroup
                         select new Grouping<string, OpenRestaurant>(storeGroup.Key, storeGroup);

            StoresGrouped.ReplaceRange(sorted);
        }
    }
}
