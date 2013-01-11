using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NerdDinner.Models
{
    public class DinnerFormViewModel
    {

        private static readonly string[] _countries = new[] {
                "USA",
                "Afghanistan",
                "Akrotiri",
                "Albania",
                "Zimbabwe"
            };

        public Dinner Dinner { get; set; }
        public SelectList Countries { get; set; }

        public DinnerFormViewModel(Dinner dinner)
        {
            Dinner = dinner;
            Countries = new SelectList(_countries, dinner.Country);

        }
    }
}