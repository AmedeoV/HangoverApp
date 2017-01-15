using System;
using System.Collections.Generic;
using HangoverApp.Models;
using HangoverApp.ViewModels;
using HangoverApp.Views;
using Xamarin.Forms;
using HangoverApp;
using MvvmHelpers;

namespace HangoverApp
{
	public partial class StoresPage : ContentPage
	{
		StoresViewModel viewModel;
        readonly IDataStore dataStore;
        public ObservableRangeCollection<Restaurant> Stores { get; set; }
        public ObservableRangeCollection<Grouping<string, Store>> StoresGrouped { get; set; }
        public bool ForceSync { get; set; }
        public StoresPage (List<Restaurant> restaurants)
		{
            dataStore = DependencyService.Get<IDataStore>();
            Stores = new ObservableRangeCollection<Restaurant>();
            StoresGrouped = new ObservableRangeCollection<Grouping<string, Store>>();
            BackgroundImage = "RestaurantsBackground.png";
            InitializeComponent();
            BindingContext = viewModel = new StoresViewModel (this);

            //viewModel.GetStoresCommand.Execute(restaurants);

            Stores.ReplaceRange(restaurants);
            viewModel.GetStoresCommand(restaurants);

            //viewModel.ForceSync = true;
            NewStore.Clicked += async (sender, e) =>
            {
                Page page = new Page();
                await page.Navigation.PushModalAsync(new MainListPage());
                if (Device.OS == TargetPlatform.Android)
                    Application.Current.MainPage = new MainListPage();
            };

            //StoreList.ItemSelected += async (sender, e) => 
            //{
            //	if(StoreList.SelectedItem == null)
            //		return;


            //	await Navigation.PushAsync(new StorePage(e.SelectedItem as Store));


            //	StoreList.SelectedItem = null;
            //};
        }

		public async void OnDelete (object sender, EventArgs e) {
			var mi = ((MenuItem)sender);

			var result = await DisplayAlert ("Delete?", "Are you sure you want to remove this store?", "Yes", "No");
            if (result)
            {
                await viewModel.DeleteStore(mi.CommandParameter as Store);
            }
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			if (viewModel.Stores.Count > 0 || viewModel.IsBusy)
				return;
            //viewModel.GetStoresCommand.Execute (null);

		}

	}
}

