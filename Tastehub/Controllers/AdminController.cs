using TastehubModel.ViewModel;
using TastehubService.MenuNamespace;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tastehub.Service.BookingService;
using Tastehub.Web.Helper;
using Utility;

namespace Tastehub.Controllers
{
    [CustomAuthorize]
    [RoleAuthorize(AppConstant.RoleAdmin,AppConstant.RoleSuperAdmin)]
    public class AdminController : Controller
    {
        // GET: Admin
        MenuService menuService = new MenuService();
        BookingService _bookingService = new BookingService();
        public ActionResult Dashboard()
        {
            List <BookingReqestViewModel> bookings = _bookingService.GetBookingList().ToList();
            ViewBag.TotalOrder = menuService.GetAllPayment().Count();
            ViewBag.Totalbooking = bookings.Count();
            ViewBag.Todaysbooking = bookings.Where(x=>x.Date.Date==DateTime.Now.Date).Count();
            ViewBag.CreatedToday = bookings.Where(x => x.Date.Date == DateTime.Now.Date).Count();
            return View();
        }
        public ActionResult ManageWebsiteContent(int ?pageSize,int? page)
        {
            
           int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);
            
            ViewBag.PageSize = pageDataSize;

            var list = menuService.GetAllSubMenu().ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_WebsiteContent", list) : View(list);
            
        }
        [HttpGet]
        public ActionResult AddWebsiteContent(int data)
        {
            SubMainMenuViewModel objSubMenu = new SubMainMenuViewModel();
            if (data != 0 )
            {
                objSubMenu = menuService.GetSubMenuById((int)data);
                return View(objSubMenu);
            }
            else
            {
                return View();
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddWebsiteContent(SubMainMenuViewModel ObjSubMenu)
        {
            var result = menuService.UpdateSubMenuContent(ObjSubMenu);
            if (result)
            {
                return RedirectToAction("ManageWebsiteContent", new { });
            }
            return View();
        }
        public ActionResult ManageGallery(int? pageSize, int? page)
        {

            int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);

            ViewBag.PageSize = pageDataSize;

            var list = menuService.GetAllGallery().ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Gallery", list) : View(list);

        }
        [HttpGet]
        public ActionResult AddGallery(int data)
        {
            ViewBag.AlbumData = menuService.GetAllAlbum().Where(x => x.IsActive == true);
            GalleryViewModel objGallery = new GalleryViewModel();
            if (data != 0)
                objGallery = menuService.GetGalleryById((int)data);
            return View(objGallery);
        }
        [HttpPost]
        public ActionResult AddGallery(GalleryViewModel ObjGallery, HttpPostedFileBase galleryImage)
        {
            bool result; 
            if (galleryImage != null && galleryImage.ContentLength > 0)
            {
                string pic = $@"{Guid.NewGuid()}.jpg";
                string ImagePath = ConfigurationManager.AppSettings["ImagePath"].ToString();
                string path = System.IO.Path.Combine(Server.MapPath(ImagePath), pic);
                galleryImage.SaveAs(path);
                ObjGallery.GalleryPath = (ImagePath.Replace("~", "..")) + pic;
            }
            if (ObjGallery.id==0)
                result = menuService.SaveGallery(ObjGallery);
            else
                result = menuService.UpdateGallery(ObjGallery);
            if (result)
            {
                return RedirectToAction("ManageGallery", new { });
            }
            return View();
        }
        public ActionResult ManageAlbum(int? pageSize, int? page)
        {

            int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);
            ViewBag.PageSize = pageDataSize;
            var list = menuService.GetAllAlbum().ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Album", list) : View(list);

        }
        [HttpGet]
        public ActionResult AddAlbum(int data)
        {
            AlbumViewModel ObjAlbum = new AlbumViewModel();
            if (data != 0)
            {
                ObjAlbum = menuService.GetAlbumById((int)data);
                return View(ObjAlbum);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddAlbum(AlbumViewModel ObjAlbum)
        {
            bool result;
            if (ObjAlbum.id == 0)
                result = menuService.SaveAlbum(ObjAlbum);
            else
                result = menuService.UpdateAlbum(ObjAlbum);
            if (result)
            {
                return RedirectToAction("ManageAlbum", new { });
            }
            return View();
        }
        [HttpGet]
        public ActionResult DeleteGalleryImage(int data)
        {
            bool result=false;
            if (data != 0)
                result = menuService.DeleteGalleryItemById(data, UserAuthenticate.LogId);
            if (result)
            {
                return RedirectToAction("ManageGallery", new { });
            }
            return View();
        }
        public ActionResult ManagePayment(int? pageSize, int? page)
        {

            int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);

            ViewBag.PageSize = pageDataSize;

            var list = menuService.GetAllPayment().ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Payment", list) : View(list);

        }
        public ActionResult UserManual()
        {
            //string ImagePath = ConfigurationManager.AppSettings["ProductImage"].ToString();
            //string path = System.IO.Path.Combine(Server.MapPath(ImagePath), "ShoppingDoc.doc");
            //string docpath = "https://docs.google.com/gview?url=http://remote.url.tld/path/to/document.doc&embedded=true";
            //docpath=docpath.Replace("http://remote.url.tld/path/to/document.doc", path);
            ViewBag.docpath = "https://docs.google.com/gview?url=http://indianlotus.co.nz/content/ProductImage/ShoppingDoc.docx&embedded=true ";
          //  ViewBag.docpath = docpath;
            return View();
        }
    }
}