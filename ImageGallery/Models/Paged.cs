using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageGallery.Models
{
    public class Paged
    {
        public List<Photos> Images { get; set; }
        public List<Users> Users { get; set; }
        public List<UserPosts> Posts { get; set; }
        public Int32 CurrentPage { get; set; }
        public Int32 PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
}