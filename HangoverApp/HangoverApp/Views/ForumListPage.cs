using HangoverApp.Helper;
using HangoverApp.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class ForumListPage : ContentView
    {
        public ForumListPage()
        {

            var forumlist = new ListView
            {
                HasUnevenRows = false,
                ItemTemplate = new DataTemplate(typeof(CustomListStyle)),
                ItemsSource = ForumListData.GetData(),
                BackgroundColor = Color.White,
                RowHeight = 50,
            };

            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White,
                Children = { forumlist }
            };

            forumlist.ItemSelected += (sender, e) =>
            {
                var selectedObject = e.SelectedItem as HangoverApp.Models.ForumType;

                var forumPage = new ForumDetailListPage(selectedObject.title, selectedObject.ForumId);
                Navigation.PushAsync(forumPage);
            };
        }
    }
}
