using MittalSquash.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MittalSquash.Models;
using System.Data.Entity;

namespace MittalSquash.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly MittalSquashEntities db = new MittalSquashEntities();
        // GET: Profile
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var bookings = db.Events.Where(x => x.UserId == currentUserId);
            var user = db.AspNetUsers.FirstOrDefault(x => x.Id == currentUserId);
            var userProfile = db.UserProfiles.FirstOrDefault(x => x.UserId == currentUserId);

            return View(new UserDetails
            {
                User = user,
                UserProfile = userProfile,
                Bookings = bookings
            });
        }
        public ActionResult Update(UserDetails details)
        {
            var userProfile = details.UserProfile;
            if (userProfile.ProfileId == 0)
            {
                userProfile.UserId = User.Identity.GetUserId();
                db.UserProfiles.Add(details.UserProfile);
            }
            else
            {
                db.Entry(userProfile).State = EntityState.Modified;
            }
            db.SaveChanges();

            return RedirectToAction("Index", "Profile");
        }
        }
    }
