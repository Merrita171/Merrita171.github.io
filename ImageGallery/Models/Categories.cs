using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageGallery.Models
{
    public class Categories
    {
        [Key]
        public int TypeId { get; set; }

        [Display(Name = "Title")]
        [Required]
        public String Description { get; set; }

        [Display(Name = "Image Path")]
        public String ImagePath { get; set; }

        [Display(Name = "Thumb Path")]
        public String ThumbPath { get; set; }


        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
    }
}