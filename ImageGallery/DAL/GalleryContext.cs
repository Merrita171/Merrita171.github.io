using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ImageGallery.Models;

namespace ImageGallery.DAL
{
    public class GalleryContext : DbContext
    {
        public GalleryContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<GalleryContext>
                (new DropCreateDatabaseIfModelChanges<GalleryContext>());
        }

        public DbSet<Photos> Photos { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserPosts> UserPosts { get; set; }
        public DbSet<Categories> Categories { get; set; }
    }
}