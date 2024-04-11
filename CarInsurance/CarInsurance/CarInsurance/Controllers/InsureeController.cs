using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insurees insurees = db.Insurees.Find(id);
            if (insurees == null)
            {
                return HttpNotFound();
            }
            return View(insurees);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insurees insurees)
        {
            if (ModelState.IsValid)
            {
                //Start with a base of $50 / month.
                decimal baseQuote = 50.0m;

                //If the user is 18 or under, add $100 to the monthly total.
                if (DateTime.Now.Year - insurees.DateOfBirth.Year <= 18)
                {
                    baseQuote += 100.0m;
                }

                //If the user is from 19 to 25, add $50 to the monthly total.
                int age = DateTime.Now.Year - insurees.DateOfBirth.Year;
                if (age > 19 && age < 25)
                {
                    baseQuote += 50.0m;
                }

                //If the user is 26 or older, add $25 to the monthly total. Double check your code to ensure all ages are covered.
                if (DateTime.Now.Year - insurees.DateOfBirth.Year >= 25)
                {
                    baseQuote += 25.0m;
                }

                //If the car's year is before 2000, add $25 to the monthly total.
                if (insurees.CarYear < 2000 || insurees.CarYear > DateTime.Now.Year)
                {
                    baseQuote += 25.0m;
                }

                //If the car's year is after 2015, add $25 to the monthly total.
                if (insurees.CarYear < 2015 || insurees.CarYear > DateTime.Now.Year)
                {
                    baseQuote += 25.0m;
                }

                //If the car's Make is a Porsche, add $25 to the price.
                if (insurees.CarMake.ToLower() == "porsche")
                {
                    baseQuote += 25.0m;
                }

                //If the car's Make is a Porsche and its model is a 911 Carrera, add an additional $25 to the price.
                //(Meaning, this specific car will add a total of $50 to the price.)
                if (insurees.CarMake.ToLower() == "porsche" && insurees.CarModel.ToLower() == "911 carrera")
                {
                    baseQuote += 25.0m;
                }

                //Add $10 to the monthly total for every speeding ticket the user has.
                baseQuote += insurees.SpeedingTickets * 10.0m;

                //If the user has ever had a DUI, add 25 % to the total.
                if (insurees.DUI)
                {
                    baseQuote *= 1.25m;
                }

                //If it's full coverage, add 50% to the total.
                if (insurees.CoverageType.ToString() == "full")
                {
                    baseQuote *= 1.5m;
                }

                insurees.Quote = baseQuote;
                db.Insurees.Add(insurees);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insurees);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insurees insurees = db.Insurees.Find(id);
            if (insurees == null)
            {
                return HttpNotFound();
            }
            return View(insurees);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insurees insurees)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insurees).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insurees);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insurees insurees = db.Insurees.Find(id);
            if (insurees == null)
            {
                return HttpNotFound();
            }
            return View(insurees);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insurees insurees = db.Insurees.Find(id);
            db.Insurees.Remove(insurees);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Admin()
        {
            var insurees = db.Insurees.ToList();
            return View(insurees);
        }

    }
}
