using MittalSquash.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MittalSquash.Models
{
    public class EventBooking
    {
        public string Time { get; set; }
        public int EventId { get; set; }
        public bool booked { get; set; }
        public bool isOwner { get; set; }
        public bool canBook { get; set; }
        public string UserName { get; set; }
        public Event Event { get; set; }
    }
}