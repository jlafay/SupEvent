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
    public class UserController : Controller
    {
        private SupEventContext db = new SupEventContext();

        public ActionResult ShowProfile()
        {
            if (Session["uid"] != null)
            {
                User newUser = db.Users.Find(Session["uid"]);
                return View(newUser);
            }
            return RedirectToAction("Login"); 
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "UserId,Name,Nickname,Password")] User newUser)
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
                return RedirectToAction("Login");
            }
            return View(newUser);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Nickname,Password")] User newUser)
        {
            var query = from u in db.Users
                        where u.Nickname == newUser.Nickname && u.Password == newUser.Password
                        select u;

            foreach (var result in query)
            {
                if (result.Password == newUser.Password)
                {
                    Session["uid"] = result.UserId;
                    Session["username"] = result.Nickname;
                    Session["role"] = result.Role;
                    return RedirectToAction("ShowProfile");
                }
            }
            ViewData["message"] = "Please check your username and your password.";
            return View();
        }

        public ActionResult Logout()
        {
            if (Session["uid"] != null)
            {
                Session["uid"] = null;
                Session["username"] = null;
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }

        public ActionResult Edit(int? id)
        {
            if (Session["uid"] != null)
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
                return View(newUser);
            }
            return RedirectToAction("Login"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            if (Session["uid"] != null)
            {
                String sesName = Session["username"].ToString();
                User query = (from u in db.Users
                              where u.Nickname == sesName
                              select u).First();

                if (Request.Form["Name"] != null)
                {
                    query.Name = Request.Form["Name"];
                }

                if (Request.Form["Nickname"] != null)
                {
                    query.Nickname = Request.Form["Nickname"];
                    Session["username"] = Request.Form["Nickname"];
                }
                db.SaveChanges();
                return RedirectToAction("ShowProfile");
            }
            return RedirectToAction("Login");
        }

        public ActionResult Reset(int? id)
        {
            if (Session["uid"] != null)
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
                return View(newUser);
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reset()
        {
            if (Session["uid"] != null)
            {
                String sesName = Session["username"].ToString();
                User query = (from u in db.Users
                              where u.Nickname == sesName
                              select u).First();

                if (Request.Form["Password"] != null)
                {
                    query.Password = Request.Form["Password"];
                }
                db.SaveChanges();
                return RedirectToAction("ShowProfile");
            }
            return RedirectToAction("Login");
        }
        
        public ActionResult Friends()
        {
            if (Session["uid"] != null)
            {
                String sesName = Session["username"].ToString();
                User usr = (from u in db.Users
                            where u.Nickname == sesName
                            select u).First();
                List<Friend> friendList = usr.FriendList;
                return View(friendList);
            }
            return RedirectToAction("Login");
        }

        public ActionResult AddFriend(int? id)
        {
            if (Session["uid"] != null)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFriend()
        {
            if (Session["uid"] != null)
            {
                if (Request.Form["Nickname"] != null)
                {
                    String sesName = Session["username"].ToString();
                    User usr = (from u in db.Users
                                where u.Nickname == sesName
                                select u).First();
                    try
                    {
                        String friendName = Request.Form["Nickname"].ToString();
                        User fri = (from u in db.Users
                                    where u.Nickname == friendName
                                    select u).First();
                        if (fri != null)
                        {
                            List<Friend> frList = usr.FriendList;
                            foreach (Friend f in frList)
                            {
                                if (f.Name == fri.Name)
                                {
                                    return RedirectToAction("Friends");
                                }
                            }
                            Friend newFri = new Friend();
                            newFri.Name = fri.Name;
                            newFri.Nickname = fri.Nickname;

                            List<Friend> friendList = usr.FriendList;
                            friendList.Add(newFri);
                            usr.FriendList = friendList;
                        }
                        db.SaveChanges();
                    }
                    catch
                    {
                        return RedirectToAction("Friends");
                    }
                }
                return RedirectToAction("Friends"); 
            }
            return RedirectToAction("Login"); 
        }

        public ActionResult DeleteFriend(int? id)
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
                List<Friend> frList = usr.FriendList;

                foreach (Friend f in frList)
                {
                    if (f.FriendId == id)
                    {
                        frList.Remove(f);
                        usr.FriendList = frList;
                        db.Friends.Remove(f);
                        db.SaveChanges();
                        return RedirectToAction("Friends");
                    }
                }
                return RedirectToAction("Friends");
            }
            return RedirectToAction("Login");
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
