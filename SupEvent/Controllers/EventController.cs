using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _SupEvent.Models;

namespace _SupEvent.Controllers
{
    public class EventController : Controller
    {
        private SupEventContext db = new SupEventContext();

        public ActionResult Events()
        {
            if (Session["uid"] != null)
            {
                String sesName = Session["username"].ToString();
                User usr = (from u in db.Users
                            where u.Nickname == sesName
                            select u).First();
                List<Event> eventList = usr.EventList;
                return View(eventList);
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult AddEvent(int? id)
        {
            if (Session["uid"] != null)
            {
                return View();
            }
            return RedirectToAction("../User/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEvent()
        {
            if (Session["uid"] != null)
            {
                if (ModelState.IsValid)
                {
                    String testName = Request.Form["Name"].ToString();
                    try
                    {
                        User name = (from u in db.Users
                                     where u.Name == testName
                                     select u).First();

                        if (name != null)
                        {
                            return RedirectToAction("Events");
                        }
                    }
                    catch { }

                    String sesName = Session["username"].ToString();
                    User usr = (from u in db.Users
                            where u.Nickname == sesName
                            select u).First();
                    
                    Event newEvent = new Event();
                    newEvent.Creator = sesName;
                    newEvent.Status = "pending";
                    newEvent.Name = Request.Form["Name"].ToString();
                    newEvent.Type = Request.Form["Type"].ToString();
                    newEvent.Address = Request.Form["Address"].ToString();
                    newEvent.Description = Request.Form["Description"].ToString();
                    DateTime dt = Convert.ToDateTime(Request.Form["EventDate"]); 
                    newEvent.EventDate = dt;
                    DateTime t = Convert.ToDateTime(Request.Form["EventTime"]);
                    newEvent.EventTime = t;

                    List<Event> eventList = usr.EventList;
                    eventList.Add(newEvent);
                    usr.EventList = eventList;
                    db.SaveChanges();
                }
                return RedirectToAction("Events");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult DeleteEvent(int? id)
        {
            if (Session["uid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                String sesName = Session["username"].ToString();
                User usr = (from u in db.Users
                            where u.Nickname == sesName
                            select u).First();
                List<Event> evList = usr.EventList;

                foreach (Event e in evList)
                {
                    if (e.EventId == id)
                    {
                        evList.Remove(e);
                        usr.EventList = evList;
                        db.Events.Remove(e);
                        db.SaveChanges();
                        return RedirectToAction("Events");
                    }
                }
                return RedirectToAction("Events");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult EditEvent(int? id)
        {
            if (Session["uid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Event ev = db.Events.Find(id);
                if (ev == null)
                {
                    return HttpNotFound();
                }
                return View(ev);
            }
            return RedirectToAction("../User/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent([Bind(Include = "EventId")] Event transEvent)
        {
            if (Session["uid"] != null)
            {
                int id = transEvent.EventId;

                String sesName = Session["username"].ToString();
                User usr = (from u in db.Users
                            where u.Nickname == sesName
                            select u).First();
                List<Event> evList = usr.EventList;

                foreach (Event e in evList)
                {
                    if (e.EventId == id)
                    {
                        if (Request.Form["Name"].ToString() != null)
                        {
                            e.Name = Request.Form["Name"].ToString();
                        }
                        if (Request.Form["Type"].ToString() != null)
                        {
                            e.Type = Request.Form["Type"].ToString();
                        }
                        if (Request.Form["Address"].ToString() != null)
                        {
                            e.Address = Request.Form["Address"].ToString();
                        }
                        if (Request.Form["Description"].ToString() != null)
                        {
                            e.Description = Request.Form["Description"].ToString();
                        }
                        if (Request.Form["EventDate"].ToString() != null)
                        {
                            DateTime dt = Convert.ToDateTime(Request.Form["EventDate"]);
                            e.EventDate = dt;
                        }
                        if (Request.Form["EventTime"].ToString() != null)
                        {
                            DateTime t = Convert.ToDateTime(Request.Form["EventTime"]);
                            e.EventTime = t;
                        }
                        db.SaveChanges();
                        return RedirectToAction("Events");
                    }
                }
                return RedirectToAction("Events");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult Guests(int? id)
        {
            if (Session["uid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Event evt = (from e in db.Events
                             where e.EventId == id
                             select e).First();
                if (evt.Status == "open")
                {
                    List<Guest> guestList = evt.GuestList;
                    return View(guestList);
                }
                return RedirectToAction("Events");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult AddGuest(int? id)
        {
            if (Session["uid"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Event evt = (from e in db.Events
                             where e.EventId == id
                             select e).First();
                if (evt.Status == "open")
                {
                    TempData["id"] = id;
                    return View();
                }
                return RedirectToAction("Events");
            }
            return RedirectToAction("../User/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGuest()
        {
            if (Session["uid"] != null)
            {
                int id = (int)TempData["id"];
                if (Request.Form["Nickname"] != null)
                {
                    Event evt = (from e in db.Events
                                 where e.EventId == id
                                 select e).First();
                    try
                    {
                        String usrName = Session["username"].ToString();
                        String friendName = Request.Form["Nickname"].ToString();
                        User usr = (from u in db.Users
                                    where u.Nickname == usrName
                                    select u).First();
                        List<Friend> frList = usr.FriendList;
                        foreach (Friend f in frList)
                        {
                            if (f.Nickname == friendName)
                            {
                                List<Guest> gList = evt.GuestList;
                                foreach (Guest g in gList)
                                {
                                    if (g.Nickname == friendName)
                                    {
                                        return RedirectToAction("AddGuest");
                                    }
                                }
                                Guest newGu = new Guest();
                                newGu.Name = f.Name;
                                newGu.Nickname = f.Nickname;

                                List<Guest> guestList = evt.GuestList;
                                guestList.Add(newGu);
                                evt.GuestList = guestList;
                                db.SaveChanges();
                                return RedirectToAction("Guests");
                            }
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Events");
                    }
                }
                return RedirectToAction("Events");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult Fevents()
        {
            if (Session["uid"] != null)
            {
                String sesName = Session["username"].ToString();

                List<Event> eventList = new List<Event>();

                try
                {
                    var evList = from e in db.Events
                                 select e;

                    foreach (Event e in evList)
                    {
                        SupEventContext db2 = new SupEventContext();
                        List<Guest> gList = (from ev in db2.Events
                                             select ev.GuestList).First();

                        foreach (Guest g in gList)
                        {
                            if (sesName == g.Nickname)
                            {
                                eventList.Add(e);
                            }
                        }
                        db2.Dispose();
                    }
                }
                catch
                {
                    return View();
                }
                return View(eventList);
            }
            return RedirectToAction("../User/Login");
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
