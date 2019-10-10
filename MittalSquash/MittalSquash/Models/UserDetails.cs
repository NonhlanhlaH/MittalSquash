using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MittalSquash.Data;

namespace MittalSquash.Models
{
    public class UserDetails
    {
        public AspNetUser User { get; set; } //Get and Set user details
        public UserProfile UserProfile { get; set; }
        public IEnumerable<Event> Bookings { get; set; }// event for booking
    }
}