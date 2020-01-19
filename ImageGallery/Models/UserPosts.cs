using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageGallery.Models
{
    public class UserPosts
    {
        [Key]
        public int PostId { get; set; }
        public string UserName { get; set; }
        public string Comments { get; set; }
        public DateTime Createdon { get; set; }

    }
}