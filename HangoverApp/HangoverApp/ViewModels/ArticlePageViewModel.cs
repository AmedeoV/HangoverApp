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
    public class ArticlePageViewModel : ICarouselViewModel
    {
        public ContentView View
        {
            get { return new ArticleListPage(); }
        }


        public string TabText
        {
            get { return "Article"; }
        }

        public string TabIcon
        {
            get
            {
                return "article.png";
            }
        }
    }
}
