using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MittalSquash.Data;

namespace MittalSquash.Controllers
{
    [Authorize]
    public class WorkingHoursController : Controller
    {
        private MittalSquashEntities db = new MittalSquashEntities();

        // GET: WorkingHours
        public ActionResult Index()
        {
            return View(db.WorkingHours.ToList());
        }

        // GET: WorkingHours/Details/5
        public ActionResult Details(int? id) // edit event handler working hours
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkingHour workingHour = db.WorkingHours.Find(id);
            if (workingHour == null)
            {
                return HttpNotFound();
            }
            return View(workingHour);
        }

        // GET: WorkingHours/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkingHours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,WorkDay,DayOfWeek,StartTimeHour,StartTimeMinute,EndTimeHour,EndTimeMinute,isActive")] WorkingHour workingHour) //bind variables to db
        {
            if (ModelState.IsValid)
            {
                db.WorkingHours.Add(workingHour);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workingHour);
        }

        // GET: WorkingHours/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkingHour workingHour = db.WorkingHours.Find(id);
            if (workingHour == null)
            {
                return HttpNotFound();
            }
            return View(workingHour);
        }

        // POST: WorkingHours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WorkDay,DayOfWeek,StartTimeHour,StartTimeMinute,EndTimeHour,EndTimeMinute,isActive")] WorkingHour workingHour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workingHour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workingHour);
        }

        // GET: WorkingHours/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkingHour workingHour = db.WorkingHours.Find(id);
            if (workingHour == null)
            {
                return HttpNotFound();
            }
            return View(workingHour);
        }

        // POST: WorkingHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkingHour workingHour = db.WorkingHours.Find(id);
            db.WorkingHours.Remove(workingHour);
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
    }
}
