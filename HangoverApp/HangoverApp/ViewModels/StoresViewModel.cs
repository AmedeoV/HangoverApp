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
        public ObservableRangeCollection<Store> Stores { get; set; }
        public ObservableRangeCollection<Grouping<string, Store>> StoresGrouped { get; set; }
        public bool ForceSync { get; set; }
        public StoresViewModel(Page page) : base(page)
        {
            dataStore = DependencyService.Get<IDataStore>();
            Stores = new ObservableRangeCollection<Store>();
            StoresGrouped = new ObservableRangeCollection<Grouping<string, Store>>();
        }
        public Action<Store> ItemSelected { get; set; }

        Store selectedStore;
        public Store SelectedStore
        {
            get { return selectedStore; }
            set
            {
                selectedStore = value;
                OnPropertyChanged("SelectedStore");
                if (selectedStore == null)
                    return;

                if (ItemSelected == null)
                {
                    page.Navigation.PushAsync(new StorePage(selectedStore));
                    SelectedStore = null;
                }
                else
                {
                    ItemSelected.Invoke(selectedStore);
                }
            }
        }


        public async Task DeleteStore(Store store)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            try
            {
                await dataStore.RemoveStoreAsync(store);
                Stores.Remove(store);
                Sort();
            }
            catch (Exception ex)
            {
                await page.DisplayAlert("Uh Oh :(", $"Unable to remove {store?.Name ?? "Unknown"}, please try again: {ex.Message}", "OK");
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
                var stores = new List<Store>();

                foreach (var restaurant in restaurants)
                {
                    stores.Add(new Store()
                    {
                        Country = "Restaurants",
                        Name = restaurant.Name,
                        Image = "https:" + restaurant.Logo,
                        PhoneNumber = "(" + restaurant.Reviews + ")",
                        ZipCode = "Zipcode",
                        StarsRating = "https:" + restaurant.StarsImage,
                        LocationHint = restaurant.Cuisine,
                    });

                }
                
                Stores.ReplaceRange(stores);

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
                         orderby store.Country, store.City
                         group store by store.Country into storeGroup
                         select new Grouping<string, Store>(storeGroup.Key, storeGroup);

            StoresGrouped.ReplaceRange(sorted);
        }
    }
}
