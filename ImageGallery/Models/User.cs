using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageGallery.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int ? Phone { get; set; }
        public int ? CatId { get; set; }
        public string Photo { get; set; }
        public int ? PhotoId { get; set; }

    }
}