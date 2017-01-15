using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HangoverApp.Models;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class CustomCell : ViewCell
    {
        public Label nameLabel;

        public static readonly BindableProperty NameProperty =
            BindableProperty.Create("Name", typeof(string), typeof(string), "Name");
        //public static readonly BindableProperty AgeProperty =
        //    BindableProperty.Create("Age", typeof(int), typeof(int), 0);
        //public static readonly BindableProperty LocationProperty =
        //    BindableProperty.Create("Location", typeof(string), typeof(string), "Location");

        public CustomCell()
        {
            nameLabel = new Label();
        }
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        //public int Age
        //{
        //    get { return (int)GetValue(AgeProperty); }
        //    set { SetValue(AgeProperty, value); }
        //}

        //public string Location
        //{
        //    get { return (string)GetValue(LocationProperty); }
        //    set { SetValue(LocationProperty, value); }
        //}

    protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                nameLabel.Text = Name;
                //ageLabel.Text = Age.ToString();
                //locationLabel.Text = Location;
            }
        }
    }
}
