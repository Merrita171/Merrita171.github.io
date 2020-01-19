using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageGallery.Models;
using ImageGallery.DAL;
using System.Drawing;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using ImageGallery.Repository;

namespace ImageGallery.Controllers
{
    
    public class HomeController : Controller
    {
        private GalleryContext db = new GalleryContext();
        GalleryRepository repo = new GalleryRepository();
        GalleryRepository galRepo = new GalleryRepository();

        public ActionResult Search(string filter = null, int page = 1, int pageSize = 20)
        {

            IEnumerable<Photos> details = null;

            using (var client = new WebClient())
            {
                var text = client.DownloadString("http://jsonplaceholder.typicode.com/posts/1");
                Details det = JsonConvert.DeserializeObject<Details>(text);
                if (filter == null)
                {
                    ViewBag.filter = det.title;
                }
                else
                {
                    ViewBag.filter = filter;
                }

                var records = new PagedList<Photos>();
                var Users  = new List<Users>();
                ViewBag.filter = filter;

                Users = repo.GetUser(filter);
                records.TotalRecords = db.Photos.Count();
                records.Content = repo.GetPhotos(filter);
                records.CurrentPage = page;
                records.PageSize = pageSize;

                if (Users != null)
                {
                   Session["Name"] = Users.Select(x => x.Name);
                    Session["Address"] = Users.Select(x => x.Address);
                   Session["Phone"] = Users.Select(x => x.Phone);
                    Session["Email"] = Users.Select(x => x.Email);
                    Session["Photo"] = Users.Select(x => x.Photo);
                }

                details = repo.GetPhotos(filter);
                records.CurrentPage = page;
                records.PageSize = pageSize;
                return View(records);
            }
        }
        public ActionResult Index(string filter = null, int page = 1, int pageSize = 20)
        {
            var records = new PagedList<Photos>();

            ViewBag.filter = filter;

            records.Content = db.Photos
                        .Where(x => filter == null || (x.Description.Contains(filter)))
                        .OrderByDescending(x => x.PhotoId)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.Photos
                            .Where(x => filter == null || (x.Description.Contains(filter))).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }
        public ActionResult SearchAlbum(string filter = null, int page = 1, int pageSize = 20)
        {
            var records = new PagedList<Photos>();

            ViewBag.filter = filter;

            records.Content = db.Photos
                        .Where(x => (x.Description.Contains(filter)))
                        .OrderByDescending(x => x.PhotoId)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            // Count
            records.TotalRecords = db.Photos
                            .Where(x => (x.Description.Contains(filter))).Count();

            records.CurrentPage = page;
            records.PageSize = pageSize;

            return View(records);
        }
        public ActionResult SearchUser(string filter = null, int page = 1, int pageSize = 20)
        {
          

            var User = new Users();
            ViewBag.filter = filter;
            User = db.Users
           .Where(x => (x.Name == filter)).FirstOrDefault();
           
            return View(User);
            

               
        }

        public ActionResult Error()
        {
            ViewBag.Message = "User is not Found !";

            return View();
        }
        //public ActionResult Gallery(int page = 1,string search = "")
        //{
        //    var records = new PagedList();
        //    int pageSize = 10;
        //    int totalRecord = 0;
        //    if (page < 1) page = 1;
        //    int skip = (page * pageSize) - pageSize;
        //    records.Images = repo.GetPhotos(search);
        //    ViewBag.filter = search;
        //    records.CurrentPage = page;
        //    records.PageSize = pageSize;
        //    ViewBag.TotalRows = totalRecord;
        //    ViewBag.search = search;
        //    return View(records);
        //}
        //public ActionResult OurGallery(int page = 1, string search = "")
        //{
        //    var records = new Photos();
        //    int pageSize = 10;
        //    int totalRecord = 0;
        //    if (page < 1) page = 1;
        //    int skip = (page * pageSize) - pageSize;
        //    var data = repo.GetPhotos(search);
        //    foreach (var item in data)
        //    {
        //        records.ImageList.Add(item.ThumbPath);
        //    }
        //    ViewBag.filter = search;
        //    ViewBag.TotalRows = totalRecord;
        //    ViewBag.search = search;
        //    return View(records);
        //}
        //public ActionResult album(int filter )
        //{
        //    int page = 1, pageSize = 20;
        //    var records = new PagedList();
        //    var photos = new List<Photos>();
        //    var User = new List<Users>();
        //    var category = new List<Categories>();
        //    ViewBag.filter = filter;
        //    ViewBag.total = db.Categories
        //                    .Where(x => (x.TypeId == filter)).Select(x => x.Description);


        //    photos = db.Photos
        //   .Where(x => (x.TypeId == filter))
        //                .OrderByDescending(x => x.PhotoId)
        //                .Skip((page - 1) * pageSize)
        //                .Take(pageSize).ToList();

        //    // Count
        //    records.TotalRecords = Convert.ToInt16(db.Photos
        //                    .Where(x => (x.TypeId==filter)).Count());



        //    records.CurrentPage = page;
        //    records.PageSize = pageSize;
        //    records.Images = photos;
        //    records.User = User;
        //    records.Type = category;
        //    return View(records);
        //}
        public ActionResult UserDetails(string filter)
        {
            int page = 1, pageSize = 20;
           
            var User = new Users();
            ViewBag.filter = filter;
            User = db.Users
           .Where(x => (x.Name == filter)).FirstOrDefault();
                        

           return View(User);
        }
        public ActionResult category(int catId)
        {
           
            var category = new List<Categories>();
            
            category = db.Categories.Where(x => (x.TypeId == catId)).Select(x=>x).ToList();
            return View(category);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var photo = new Photos();
            return View(photo);
        }

        [HttpPost]
        public ActionResult Create(Photos photo, IEnumerable<HttpPostedFileBase> files)
        {
            if (!ModelState.IsValid)
                return View(photo);
            if (files.Count() == 0 || files.FirstOrDefault() == null)
            {
                ViewBag.error = "Please Choose a fi9le to upload !";
                return View(photo);
            }
           // int typeid= photo.PhotoId;
            var model = new Photos();
            foreach (var file in files)
            {
                if (file.ContentLength == 0) continue;
                model.UserName = photo.UserName;
                model.Description = photo.Description;
                var fileName = Guid.NewGuid().ToString();
                var extension = System.IO.Path.GetExtension(file.FileName).ToLower();

                using (var img = System.Drawing.Image.FromStream(file.InputStream))
                {
                    model.ThumbPath = String.Format("/GalleryImages/thumbs/{0}{1}", fileName, extension);
                    model.ImagePath = String.Format("/GalleryImages/{0}{1}", fileName, extension);

                    // Save thumbnail size image, 100 x 100
                    SaveToFolder(img, fileName, extension, new Size(100, 100), model.ThumbPath);

                    // Save large size image, 800 x 800
                    SaveToFolder(img, fileName, extension, new Size(600, 600), model.ImagePath);
                }
               // model.PhotoId = typeid;
                galRepo.Upload(model);

                 }

            return RedirectPermanent("/home");
        }
        [HttpGet]
        public ActionResult CreateUser()
        {
            var User = new Users();
           
            return View(User);
        }

        [HttpPost]
        public ActionResult CreateUser(Users User, IEnumerable<HttpPostedFileBase> files)
        {
            if (!ModelState.IsValid)
                return View(User);
            if (files.Count() == 0 || files.FirstOrDefault() == null)
            {
                ViewBag.error = "Please Choose a file to upload !";
                return View(User);
            }
            //int typeid = User.UserId;
            var model = new Users();
            foreach (var file in files)
            {
                if (file.ContentLength == 0) continue;

                model.Name = User.Name;
                model.Address = User.Address;
                model.Email = User.Email;
                model.Phone = User.Phone;
                var fileName = Guid.NewGuid().ToString();
                var extension = System.IO.Path.GetExtension(file.FileName).ToLower();

                using (var img = System.Drawing.Image.FromStream(file.InputStream))
                {
                    model.Photo = String.Format("/GalleryImages/thumbs/{0}{1}", fileName, extension);
                   // model.Photo = String.Format("/GalleryImages/{0}{1}", fileName, extension);

                    // Save thumbnail size image, 100 x 100
                    SaveToFolder(img, fileName, extension, new Size(100, 100), model.Photo);

                    // Save large size image, 800 x 800
                    SaveToFolder(img, fileName, extension, new Size(600, 600), model.Photo);
                }
               // model.UserId = typeid;
                galRepo.UploadUser(model);

            }

            return RedirectPermanent("/home");
        }

        [HttpGet]
        public ActionResult CreatePosts(string filter)
        {
            var UserPosts = new UserPosts();
            UserPosts.UserName = filter;
            ViewBag.name = filter;
            return View(UserPosts);
        }

        [HttpPost]
        public ActionResult CreatePosts(UserPosts posts)
        {
            if (!ModelState.IsValid)
                return View(posts);
           
            // int typeid= photo.PhotoId;
            var model = new UserPosts();
            model.UserName = posts.UserName;
            model.Comments = posts.Comments;
            model.Createdon = System.DateTime.Now;
            // model.PhotoId = typeid;
            galRepo.UploadPosts(model);

            return RedirectPermanent("/home");
        }
        public Size NewImageSize(Size imageSize, Size newSize)
        {
            Size finalSize;
            double tempval;
            if (imageSize.Height > newSize.Height || imageSize.Width > newSize.Width)
            {
                if (imageSize.Height > imageSize.Width)
                    tempval = newSize.Height / (imageSize.Height * 1.0);
                else
                    tempval = newSize.Width / (imageSize.Width * 1.0);

                finalSize = new Size((int)(tempval * imageSize.Width), (int)(tempval * imageSize.Height));
            }
            else
                finalSize = imageSize; // image is already small size

            return finalSize;
        }

        private void SaveToFolder(Image img, string fileName, string extension, Size newSize, string pathToSave)
        {
            // Get new resolution
            Size imgSize = NewImageSize(img.Size, newSize);
            using (System.Drawing.Image newImg = new Bitmap(img, imgSize.Width, imgSize.Height))
            {
                newImg.Save(Server.MapPath(pathToSave), img.RawFormat);
            }
        }

        public ActionResult Portfolio()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult UserProfile()
        {
            var User = new List<Users>();


            User = db.Users.Select(x=>x)  
                        .ToList();

          
            return View(User);
        }

        public JsonResult GetGallery()
        {
            string filter = null;
            var jsonData = new
            {
                total = 1,
                page = 1,
                records = galRepo.GetPhotos(filter).ToList().Count,
                rows = (
                  from emp in  galRepo.GetPhotos(filter).ToList()
            select new
                  {
                ImagePath  = emp.ImagePath,
                      cell = new string[] {
               emp.ImagePath.ToString()
                    }
                  }).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewComments(string filter)
        {
            var records = new PagedList<UserPosts>();

            ViewBag.filter = filter;

            records.Content = db.UserPosts
                        .Where(x => filter == null || (x.UserName.Contains(filter)))
                        .OrderByDescending(x => x.PostId)
                        
                        .ToList();

            // Count
            records.TotalRecords = db.UserPosts
                            .Where(x => filter == null || (x.UserName.Contains(filter))).Count();

            
          

            return View(records);
        }
    }

}