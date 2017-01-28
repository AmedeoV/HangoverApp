using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HangoverApp.Models;
using Xamarin.Forms;

namespace HangoverApp.ViewModels
{
    public class StoreViewModel : ViewModelBase
    {
        public OpenRestaurant OpenRestaurant { get; set; }
        public string Monday { get { return string.Format("{0} - {1}", OpenRestaurant.MondayOpen, OpenRestaurant.MondayClose); } }
        public string Tuesday { get { return string.Format("{0} - {1}", OpenRestaurant.TuesdayOpen, OpenRestaurant.TuesdayClose); } }
        public string Wednesday { get { return string.Format("{0} - {1}", OpenRestaurant.WednesdayOpen, OpenRestaurant.WednesdayClose); } }
        public string Thursday { get { return string.Format("{0} - {1}", OpenRestaurant.ThursdayOpen, OpenRestaurant.ThursdayClose); } }
        public string Friday { get { return string.Format("{0} - {1}", OpenRestaurant.FridayOpen, OpenRestaurant.FridayClose); } }
        public string Saturday { get { return string.Format("{0} - {1}", OpenRestaurant.SaturdayOpen, OpenRestaurant.SaturdayClose); } }
        public string Sunday { get { return string.Format("{0} - {1}", OpenRestaurant.SundayOpen, OpenRestaurant.SundayClose); } }


        public string Address1 { get { return OpenRestaurant.StreetAddress; } }
        public string Address2 { get { return string.Format("{0}, {1} {2}", OpenRestaurant.City, OpenRestaurant.State, OpenRestaurant.ZipCode); } }

        public StoreViewModel(OpenRestaurant openRestaurant, Page page) : base(page)
        {
            this.OpenRestaurant = openRestaurant;
        }

        Command navigateCommand;
        public Command NavigateCommand
        {
            get
            {
                return null;
                //return navigateCommand ?? (navigateCommand = new Command(() =>
                //CrossExternalMaps.Current.NavigateTo(OpenRestaurant.Name, OpenRestaurant.Latitude, OpenRestaurant.Longitude)));
            }
        }

        Command callCommand;
        public Command CallCommand
        {
            get
            {
                return callCommand ?? (callCommand = new Command(() => {
                    //var phoneCallTask = MessagingPlugin.PhoneDialer;
                    //if (phoneCallTask.CanMakePhoneCall)
                    //    phoneCallTask.MakePhoneCall(OpenRestaurant.ReviewsNumber.CleanPhone());
                }));
            }
        }

    }
}
