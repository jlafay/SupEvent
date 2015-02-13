using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _SupEvent.Models
{
    public class Friend
    {
        [DefaultValue(1)]
        [HiddenInput(DisplayValue = false)]
        public int FriendId { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}