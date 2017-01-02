using HangoverApp.Style;
using HangoverApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HangoverApp.ViewModels
{
    public class ForumViewModel : ICarouselViewModel
    {
        public ContentView View
        {
            get { return new ForumListPage(); }
        }


        public string TabText
        {
            get { return "Forum"; }
        }

        public string TabIcon
        {
            get
            {
                return "forum.png";
            }
        }
    }
}
