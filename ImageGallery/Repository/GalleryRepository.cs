using ImageGallery.DAL;
using ImageGallery.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ImageGallery.Repository
{
    public class GalleryRepository
    {
        private GalleryContext db = new GalleryContext();
        public List<Photos> Photos(string search)
        {

            var details = new List<Photos>();

            details = db.Photos.Where(x => x.Description.Contains(search) || search == null).OrderByDescending(x => x.PhotoId).ToList();
                        

            return details;
        }
        public List<Photos> GetPhotos(string search)
        {

            List<Photos> details = new List<Photos>();
              
                details = db.Photos.Where(x=>x.Description.Contains(search)|| search==null)
                            .OrderByDescending(x => x.PhotoId)
                            .ToList();
                
            return details;
            }
        public List<Users> GetUser(string name)
        {

            List<Users> details = new List<Users>();

            details = db.Users.Where(x => x.Name== name)
                        .OrderByDescending(x => x.PhotoId)
                        .ToList();

            return details;
        }
        public List<Categories> GetCategories(int catid)
        {

            List<Categories> details = new List<Categories>();
            details = db.Categories.Where(x => x.TypeId==catid)
                        .OrderByDescending(x => x.TypeId)
                        .ToList();
         
            return details;
        }
        public void  Upload(Photos photo)
        {
            photo.CreatedOn = DateTime.Now;
            db.Photos.Add(photo);
            db.SaveChanges();


         }
        public void UploadPosts(UserPosts posts)
        {
           
            db.UserPosts.Add(posts);
            db.SaveChanges();


        }
        public void UploadUser(Users user)
        {
            db.Users.Add(user);
            db.SaveChanges();


        }
        public void UploadUserPosts(UserPosts posts)
        {
           
            db.UserPosts.Add(posts);
            db.SaveChanges();


        }

    }

}