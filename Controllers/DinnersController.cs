using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NerdDinner.Helpers;
using NerdDinner.Models;

namespace NerdDinner.Controllers
{
    public class DinnersController : Controller
    {
        readonly DinnerRepository _dinnerRepository = new DinnerRepository();

        //
        // GET: /Dinners/

        public ActionResult Index(int page = 0)
        {
            const int pageSize = 1;

            var upcomingDinners = _dinnerRepository.FindUpcomingDinners();
            var paginatedDinners = new PaginatedList<Dinner>(upcomingDinners,
                                                     page,
                                                     pageSize);
            return View(paginatedDinners);
        }

        //
        // Get: /Dinners/Details/[id]

        public ActionResult Details(int id)
        {
            var dinner = _dinnerRepository.GetDinner(id);

            if(dinner == null)
                return View("NotFound");
            else
                return View(dinner);
            
        }

        //
        //Get: /Dinners/Edit/[ID]
        [Authorize]
        public ActionResult Edit(int id)
        {
            var dinner = _dinnerRepository.GetDinner(id);

            if (!dinner.IsHostedBy(User.Identity.Name))
                return View("InvalidOwner");


            return View(new DinnerFormViewModel(dinner));
        }

        //
        //POST: /Dinners/Edit/[ID]
        [HttpPost, Authorize]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            var dinner = _dinnerRepository.GetDinner(id);

            if (!dinner.IsHostedBy(User.Identity.Name))
                return View("InvalidOwner");


            if(TryUpdateModel(dinner)) {
                _dinnerRepository.Save();
                return RedirectToAction("Details", new { id=dinner.DinnerID });
            }


            return View(new DinnerFormViewModel(dinner));
        }

        //
        //GET: /Dinners/Create
        [Authorize]
        public ActionResult Create()
        {

            var dinner = new Dinner
                {
                    EventDate = DateTime.Now.AddDays(7)
                };

            return View(new DinnerFormViewModel(dinner));
        }

        //
        //POST: /Dinners/Create

        [HttpPost, Authorize]
        public ActionResult Create(Dinner dinner)
        {
            if (ModelState.IsValid)
            {
                dinner.HostedBy = User.Identity.Name;

                var rsvp = new RSVP {AttendeeName = User.Identity.Name};
                dinner.RSVPs.Add(rsvp);

                _dinnerRepository.Add(dinner);
                _dinnerRepository.Save();

                return RedirectToAction("Details", new {id = dinner.DinnerID});
            }

            return View(new DinnerFormViewModel(dinner));
        }

        //
        //HTTP GET: /Dinners/Delete/[Id]
        [Authorize]
        public ActionResult Delete(int id)
        {

            var dinner = _dinnerRepository.GetDinner(id);

            if (!dinner.IsHostedBy(User.Identity.Name))
                return View("InvalidOwner");

            if (dinner == null)
                return View("NotFound");
            else
                return View(dinner);
        }

     // 
    // HTTP POST: /Dinners/Delete/[Id]

        [HttpPost]
        public ActionResult Delete(int id, string confirmButton) {
            Dinner dinner = _dinnerRepository.GetDinner(id);
            if (dinner == null)
                return View("NotFound");
            _dinnerRepository.Delete(dinner);
            _dinnerRepository.Save();
            return View("Deleted");
}

    }
}
