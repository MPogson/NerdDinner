using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{
    public class RSVPController : Controller
    {
        private DinnerRepository dinnerRepository = new DinnerRepository();
        //
        // AJAX: /Dinners/Register/1
        [Authorize, HttpPost]
        public ActionResult Register(int id)
        {
            Dinner dinner = dinnerRepository.GetDinner(id);
            if (!dinner.IsUserRegistered(User.Identity.Name))
            {
                var rsvp = new RSVP {AttendeeName = User.Identity.Name};
                dinner.RSVPs.Add(rsvp);
                dinnerRepository.Save();
            }
            return Content("Thanks - we’ll see you there!");
        }
    }
} 