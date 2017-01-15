using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HangoverApp.Models;
using Xamarin.Forms;

namespace HangoverApp.Views
{

    public class CustomCellPage : ContentPage
    {
        public CustomCellPage(List<Restaurant> restaurants)
        {
            var relativelayout = new RelativeLayout();
            BackgroundImage = "LoginScreen.png";
            var layout = new StackLayout { Padding = 20 };
            var backgroundImage = new Image()
            {
                Aspect = Aspect.Fill,
                Source = FileImageSource.FromFile("LoginScreen.png")
            };
            relativelayout.Children.Add(backgroundImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            relativelayout.Children.Add(layout,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));
            var customCell = new DataTemplate(typeof(CustomCell));
            customCell.SetBinding(CustomCell.NameProperty, "Name");
            //customCell.SetBinding(CustomCell.AgeProperty, "Age");
            //customCell.SetBinding(CustomCell.LocationProperty, "Location");

            var listView = new ListView
            {
                ItemsSource = restaurants,
                ItemTemplate = customCell
            };

            layout.Children.Add(listView);

            Content = new ScrollView { Content = relativelayout };
        }
    }
}
