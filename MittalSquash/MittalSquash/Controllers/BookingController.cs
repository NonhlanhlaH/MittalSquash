using MittalSquash.Data;
using MittalSquash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MittalSquash.Models;

namespace MittalSquash.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly MittalSquashEntities db = new MittalSquashEntities();

        // GET: Booking
        public ActionResult Index(string date)
        {
            //Get time and Date
            var now = string.IsNullOrEmpty(date) ? DateTime.Now : Convert.ToDateTime(date);

            var todayTimes = db.WorkingHours.FirstOrDefault(x => x.DayOfWeek == (int) now.DayOfWeek);
            var startHour = todayTimes.StartTimeHour;
            var endHour = todayTimes.EndTimeHour;

            //Current date data
            var data = db.Events.Where(x => x.EventDate == now.Date).GroupBy(g => g.ResourceId);

            ViewBag.CurrentFilter = now.Date.ToString("MM/dd/yyyy");

            var bookings = new List<Booking>();
            List<Resource> resources = new List<Resource>();
            resources.Add(new Resource() {ResourceId = 0, Name = "Time"});
            resources.AddRange(db.Resources);
            
            foreach (var resource in resources)
            {

                DateTime startDate = new DateTime(now.Year, now.Month, now.Day, startHour.Value, 0, 0);
                DateTime endDate = new DateTime(now.Year, now.Month, now.Day, endHour.Value, 0, 0);

                //Resource Data
                var resourceData = data.FirstOrDefault(x => x.Key == resource.ResourceId);

                var booking = new Booking() {Resource = resource, Date = now};
                var eventBookings = new List<EventBooking>();
                while (startDate < endDate)
                {
                    EventBooking eventBooking = new EventBooking();

                    if (resourceData != null)
                    {
                        var bookedEvent = resourceData.FirstOrDefault(x => x.EventTime == startDate.ToString("HH:mm"));

                        if (bookedEvent != null)
                        {
                            eventBooking.EventId = bookedEvent.EventId;
                            eventBooking.booked = true;
                            if (bookedEvent.UserId == User.Identity.GetUserId())
                                eventBooking.isOwner = true;
                        }
                        else
                        {
                            eventBooking.EventId = 0;
                            eventBooking.booked = false;
                            eventBooking.isOwner = false;
                        }
                    }
                    else
                    {
                        eventBooking.EventId = 0;
                        eventBooking.booked = false;
                        eventBooking.isOwner = false;
                    }

                    eventBooking.Time = startDate.ToString("HH:mm tt");
                    eventBookings.Add(eventBooking);
                    startDate = startDate.AddMinutes(30);
                }

                booking.EventBookings = eventBookings;
                bookings.Add(booking);
            }

            if (TempData["mydata"] != null)
            {
                var m = TempData["mydata"] as string;
                ModelState.AddModelError(string.Empty, m);

            }

            return View(bookings);
        }

        public ActionResult SaveEvent(FormCollection data) // books court
        {
            var userId = User.Identity.GetUserId();
            var blacklist = db.UserBlacklists.Where(x => x.UserId == userId);
            if(blacklist.Count() == 3)
            {
                return View("Blacklisted");
            }
            var date = data["Date"];
            var time = data["Time"];
            var resourceId = data["Resource"];
            var bookingDate = Convert.ToDateTime(date);
            //check validations
            //Get ur booking for the day
           // var userId = User.Identity.GetUserId();
            var mybookings = db.Events.Where(x => x.EventDate == bookingDate.Date && x.UserId == userId);
            //check booking between peak times
            var peaktimes = new string[] { "17:00", "17:30", "18:00", "18:30", "19:00", "19:30", };
            var peakBooking = mybookings.Where(x => peaktimes.Contains(x.EventTime));

            if (peakBooking.Count() >= 3)
            {
                TempData["mydata"] = @"You have exceeded the (3) maximum bookings allowed during peak hours (17:00 - 20:00).";
                return RedirectToAction("Index", new { date = bookingDate.ToString("MM/dd/yyyy") });
            }
            var bookingEvent = new Event()
            {
                ResourceId = int.Parse(resourceId),
                CreateDate = DateTime.Now,
                EventDate = Convert.ToDateTime(date),
                EventTime = time.Split(' ')[0],
                Name = "booking",
                UserId = User.Identity.GetUserId()

            };
            db.Events.Add(bookingEvent);
            db.SaveChanges();

           return  RedirectToAction("Index", "Booking", new {date = bookingEvent.EventDate.ToString("MM/dd/yyyy") });
         
           // return View();
        }

        public ActionResult Absent(int eventid)
        {        
            var dbevent = db.Events.FirstOrDefault(x => x.EventId == eventid);
            db.UserBlacklists.Add(new UserBlacklist { UserId = dbevent.UserId, CreateDate = DateTime.Now });
            dbevent.Status = 0; // user absent
            db.SaveChanges();

            return RedirectToAction("Manage",new { date = dbevent.EventDate.ToString("MM/dd/yyyy") });

        }
        public ActionResult DeleteBooking(int eventid)
        {
            var dbevent = db.Events.FirstOrDefault(x => x.EventId == eventid);
            db.Events.Remove(dbevent);
            db.SaveChanges();
            return RedirectToAction("Manage",new { date = dbevent.EventDate.ToString("MM/dd/yyyy") });
           
        }
        public ActionResult Present(int eventid)
        {
            var dbevent = db.Events.FirstOrDefault(x => x.EventId == eventid);
            db.UserBlacklists.Add(new UserBlacklist { UserId = dbevent.UserId, CreateDate = DateTime.Now });
            dbevent.Status = 1;
            db.SaveChanges();

            return RedirectToAction("Manage", new { date = dbevent.EventDate.ToString("MM/dd/yyyy") });
        }
        public ActionResult Manage(string date)
        {
            //Get time and Date
            var now = string.IsNullOrEmpty(date) ? DateTime.Now : Convert.ToDateTime(date);

            var todayTimes = db.WorkingHours.FirstOrDefault(x => x.DayOfWeek == (int)now.DayOfWeek);
            var startHour = todayTimes.StartTimeHour;
            var endHour = todayTimes.EndTimeHour;

            //Current date data
            var data = db.Events.Where(x => x.EventDate == now.Date).GroupBy(g => g.ResourceId);

            ViewBag.CurrentFilter = now.Date.ToString("MM/dd/yyyy");

            var bookings = new List<Booking>();
            List<Resource> resources = new List<Resource>();
            resources.Add(new Resource() { ResourceId = 0, Name = "Time" });
            resources.AddRange(db.Resources);

            foreach (var resource in resources)//display all courts and times
            {

                DateTime startDate = new DateTime(now.Year, now.Month, now.Day, startHour.Value, 0, 0);
                DateTime endDate = new DateTime(now.Year, now.Month, now.Day, endHour.Value, 0, 0);

                //Resource Data
                var resourceData = data.FirstOrDefault(x => x.Key == resource.ResourceId);

                var booking = new Booking() { Resource = resource, Date = now };
                var eventBookings = new List<EventBooking>();
                while (startDate < endDate)
                {
                    EventBooking eventBooking = new EventBooking();

                    if (resourceData != null)
                    {
                        var bookedEvent = resourceData.FirstOrDefault(x => x.EventTime == startDate.ToString("HH:mm"));

                        if (bookedEvent != null)
                        {
                            eventBooking.EventId = bookedEvent.EventId;
                            eventBooking.booked = true;
                            eventBooking.UserName = bookedEvent.AspNetUser.UserName;
                            eventBooking.Event = bookedEvent;
                        }
                        else
                        {
                            eventBooking.EventId = 0;
                            eventBooking.booked = false;
                            eventBooking.isOwner = false;
                        }
                    }
                    else
                    {
                        eventBooking.EventId = 0;
                        eventBooking.booked = false;
                        eventBooking.isOwner = false;
                    }

                    eventBooking.Time = startDate.ToString("HH:mm tt");
                    eventBookings.Add(eventBooking);
                    startDate = startDate.AddMinutes(30);
                }

                booking.EventBookings = eventBookings;
                bookings.Add(booking);
            }

            return View(bookings);
        }
    }
}