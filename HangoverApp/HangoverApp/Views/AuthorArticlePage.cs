using HangoverApp.Helper;
using HangoverApp.Models;
using HangoverApp.Style;
using HangoverApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class AuthorArticlePage : ContentPage
    {
        AuthorDataViewModel authorDataViewModel;
        public AuthorArticlePage(string title, AuthorDataType authorDataType)
        {
            Title = title;
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            NavigationPage.SetBackButtonTitle(this, "Profile");
            authorDataViewModel = new AuthorDataViewModel(authorDataType);
            authorDataViewModel.GetAuthorDataCommand.Execute(null);
            var activityIndicator = new ActivityIndicator
            {
                Color = Color.Gray,
            };
            activityIndicator.SetBinding(IsVisibleProperty, "IsBusy");
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            var authorArticlelist = new ListView
            {
                HasUnevenRows = false,
                ItemTemplate = new DataTemplate(typeof(CPListCell)),
                ItemsSource = authorDataViewModel.AutorItems,
                BackgroundColor = Color.White,
                RowHeight = 120,
            };

            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White,
                Children = { authorArticlelist }
            };

            authorArticlelist.ItemSelected += (sender, e) =>
            {
                var selectedObject = e.SelectedItem as HangoverApp.Models.Item;

                var WebViewPage = new WebViewPage(title, selectedObject.websiteLink.HttpUrlFix());
                Navigation.PushAsync(WebViewPage);
            };
        }
    }
}
