using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _SupEvent.Models
{
    public class Guest
    {
        [DefaultValue(1)]
        [HiddenInput(DisplayValue = false)]
        public int GuestId { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public int EventId { get; set; }

        public virtual Event Event { get; set; }
    }
}