using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class OfflinePage : ContentPage
    {
        public OfflinePage()
        {
            var layout = new StackLayout { Padding = 20 };

            BackgroundImage = "LoginScreen.png";
            var button = new Button()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                Scale = 1.5,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = "Check Connection",
                BackgroundColor = Color.FromHex("#8BC34A"),
                InputTransparent = true,
                IsEnabled = true
            };
            layout.Children.Add(button);
            this.Content = layout;
        }
        
    }
}
