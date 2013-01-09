using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdDinner.Models
{
    public class DinnerRepository
    {

        private readonly NerdDinnerEntities _entities = new NerdDinnerEntities();

        public IQueryable<Dinner> FindAllDinners()
        {
            return _entities.Dinners;
        }

        public IQueryable<Dinner> FindUpcomingDinners()
        {
            return from dinner in _entities.Dinners
                   where dinner.EventDate > DateTime.Now
                   orderby dinner.EventDate
                   select dinner;
        }

        public Dinner GetDinner(int id)
        {
            return _entities.Dinners.FirstOrDefault(d => d.DinnerID == id);
        }

        public void Add(Dinner dinner)
        {
            _entities.Dinners.Add(dinner);
        }

        public void Delete(Dinner dinner)
        {

            foreach (var rsvp in dinner.RSVPs)
            {
                _entities.RSVPs.Remove(rsvp);
            }
            _entities.Dinners.Remove(dinner);
        }

        public void Save()
        {
            _entities.SaveChanges();
        }
    }
}