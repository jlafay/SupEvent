using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _SupEvent.Models
{
    public class User
    {
        [DefaultValue(1)]
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public string Password { get; set; }

        public virtual List<Friend> FriendList { get; set; }

        public virtual List<Event> EventList { get; set; }

        public string Role { get; set; }

    }
}