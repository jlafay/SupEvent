using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using _SupEvent.Models;

namespace _SupEvent.Controllers
{
    public class AdminController : Controller
    {
        private SupEventContext db = new SupEventContext();

        public ActionResult Index()
        {
            if (Session["uid"] != null)
            {
                if (Session["role"].ToString() == "Admin")
                {
                    return View();
                }
                return RedirectToAction("../User/Login");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult UserManagement()
        {
            if (Session["uid"] != null)
            {
                if (Session["role"].ToString() == "Admin")
                {
                    var usr = from u in db.Users
                              select u;
                    return View(usr);
                }
                return RedirectToAction("../User/Login");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult EventManagement()
        {
            if (Session["uid"] != null)
            {
                if (Session["role"].ToString() == "Admin")
                {
                    var evt = from e in db.Events
                              select e;
                    return View(evt);
                }
                return RedirectToAction("../User/Login");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult Authorize(int? id)
        {
            if (Session["uid"] != null)
            {
                if (Session["role"].ToString() == "Admin")
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
                    Event evt = (from e in db.Events
                                 where e.EventId == id
                                 select e).First();
                    evt.Status = "open";
                    db.SaveChanges();
                    return RedirectToAction("EventManagement");
                }
                return RedirectToAction("../User/Login");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult PassAdmin(int? id)
        {
            if (Session["uid"] != null)
            {
                if (Session["role"].ToString() == "Admin")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    User usr = db.Users.Find(id);
                    if (usr == null)
                    {
                        return HttpNotFound();
                    }
                    usr.Role = "Admin";
                    db.SaveChanges();
                    return RedirectToAction("UserManagement");
                }
                return RedirectToAction("../User/Login");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult DeleteUser(int? id)
        {
            if (Session["uid"] != null)
            {
                if (Session["role"].ToString() == "Admin")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    User usr = db.Users.Find(id);
                    if (usr == null)
                    {
                        return HttpNotFound();
                    }
                    db.Users.Remove(usr);
                    db.SaveChanges();
                    return RedirectToAction("UserManagement");
                }
                return RedirectToAction("../User/Login");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult ResetPassword(int? id)
        {
            if (Session["uid"] != null)
            {
                if (Session["role"].ToString() == "Admin")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    User newUser = db.Users.Find(id);
                    if (newUser == null)
                    {
                        return HttpNotFound();
                    }
                    TempData["id"] = id;
                    return View(newUser);
                }
                return RedirectToAction("../User/Login");
            }
            return RedirectToAction("../User/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword()
        {
            if (Session["uid"] != null)
            {
                if (Session["role"].ToString() == "Admin")
                {
                    int id = (int)TempData["id"];
                    User usr = db.Users.Find(id);
                    if (usr == null)
                    {
                        return HttpNotFound();
                    }

                    if (Request.Form["Password"] != null)
                    {
                        String pass = Request.Form["Password"].ToString();
                        usr.Password = pass;
                        db.SaveChanges();
                    }
                    return RedirectToAction("UserManagement");
                }
                return RedirectToAction("../User/Login");
            }
            return RedirectToAction("../User/Login");
        }

        public ActionResult AddUser(int? id)
        {
            if (Session["uid"] != null)
            {
                if (Session["role"].ToString() == "Admin")
                {
                    return View();
                }
                return RedirectToAction("../User/Login");
            }
            return RedirectToAction("../User/Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser([Bind(Include = "UserId,Name,Nickname,Password")] User newUser)
        {
            if (Session["uid"] != null)
            {
                if (Session["role"].ToString() == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        String testName = Request.Form["Name"].ToString();
                        String testNickname = Request.Form["Nickname"].ToString();

                        try
                        {
                            User name = (from u in db.Users
                                         where u.Name == testName
                                         select u).First();

                            if (name != null)
                            {
                                ViewData["message"] = "This name is already used";
                                return View();
                            }
                        }
                        catch { }

                        try
                        {
                            User nickname = (from u in db.Users
                                             where u.Nickname == testNickname
                                             select u).First();

                            if (nickname != null)
                            {
                                ViewData["message"] = "This nickname is already used";
                                return View();
                            }
                        }
                        catch { }

                        db.Users.Add(newUser);
                        newUser.Role = "User";
                        db.SaveChanges();
                        return RedirectToAction("UserManagement");
                    }
                    return View(newUser);
                }
                return RedirectToAction("../User/Login");
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
