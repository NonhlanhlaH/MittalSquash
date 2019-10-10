using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MittalSquash.Data;

namespace MittalSquash.Models
{
    public class Booking
    {
        public DateTime Date { get; set; }
        public Resource Resource { get; set; }
        public List<EventBooking> EventBookings { get; set; }
    }
}