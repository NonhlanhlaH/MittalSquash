using MittalSquash.Data;
using MittalSquash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MittalSquash.Controllers
{
    public class BookingHomeController : Controller
    {
        private readonly MittalSquashEntities db = new MittalSquashEntities();
        // GET: BookingHome
        public ActionResult Index(string date)
        {
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

            foreach (var resource in resources)
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