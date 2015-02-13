using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _SupEvent.Models
{
    public class Event
    {
        [DefaultValue(1)]
        [HiddenInput(DisplayValue = false)]
        public int EventId { get; set; }

        [StringLength(50, ErrorMessage = "La taille maximale est de : {0}{1}")]
        public string Name { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Event date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        [Display(Name = "Event time")]
        [DataType(DataType.Time)]
        public DateTime EventTime { get; set; }

        public string Creator { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual List<Guest> GuestList { get; set; }

    }
}
