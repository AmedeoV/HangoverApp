using HangoverApp.Helper;
using HangoverApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HangoverApp.Style
{
    public class DetailsView : ContentView
    {
        public DetailsView(MyProfile myProfile = null)
        {
            var name = new Label()
            {
                Text = myProfile.displayName,
                FontSize = 20,
                FontFamily = Device.OnPlatform("HelveticaNeue-Bold", "sans-serif-black", null),
                XAlign = TextAlignment.Center,
                TextColor = Color.Black
            };

            var where = new Label()
            {
                Text = myProfile.country,
                FontSize = 12,
                FontFamily = Device.OnPlatform("HelveticaNeue-Bold", "sans-serif-black", null),
                XAlign = TextAlignment.Center,
                TextColor = Color.FromHex("#666")
            };

            var bio = new Label()
            {
                Text = myProfile.biography.ToString().TruncateCharAtIndex(".", 2),
                FontSize = 14,
                FontFamily = Device.OnPlatform("HelveticaNeue", "sans-serif", null),
                XAlign = TextAlignment.Center,
                TextColor = Color.Black
            };

            var stack = new StackLayout()
            {
                Padding = new Thickness(20, 10),
                Children = {
                    name,
                    where,
                    bio
                }
            };

            Content = stack;
        }
    }
}
