using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangoverApp.Models
{
    public class Restaurant
    {
        public string Name { get; set; }
        public string Cuisine { get; set; }
        public string Logo { get; set; }
        public string StarsImage { get; set; }
        public string Reviews { get; set; }
        public string Distance { get; set; }
        public string Url { get; set; }
        public string RestaurantDetails{ get; set; }
        public string MinimumSpend { get; set; }
        public bool IsPreorder { get; set; }
    }
}
