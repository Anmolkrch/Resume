using TastehubModel.ViewModel;
using TastehubService.Service.ItemService;
using PagedList;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Tastehub.Web.Helper;
using Utility;

namespace Tastehub.Controllers
{
    [CustomAuthorize]
    [RoleAuthorize(AppConstant.RoleAdmin, AppConstant.RoleSuperAdmin)]
    public class ItemController : Controller
    {
        // GET: Item
        ItemService itemService = new ItemService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManageItems()
        {
            var product = itemService.GetItemImage();
            return View(product);

        }
        public ActionResult ManageProducts(int? pageSize, int? page)
        {

            int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);
            ViewBag.PageSize = pageDataSize;
            var product = itemService.GetItemList(0).ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Items", product) : View(product);

        }
        [HttpGet]
        public ActionResult AddProduct(int data)
        {
            ProductViewModel objProduct = new ProductViewModel();
            ViewBag.Category = itemService.GetCategoryList();
            if (data != 0)
                objProduct = itemService.GetProductById((int)data);
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult AddProduct(ProductViewModel ObjProduct, HttpPostedFileBase galleryImage)
        {
            bool result;
            if (galleryImage != null && galleryImage.ContentLength > 0)
            {
                string pic = $@"{Guid.NewGuid()}.jpg";
                string ImagePath = ConfigurationManager.AppSettings["ImagePath"].ToString();
                string path = System.IO.Path.Combine(Server.MapPath(ImagePath), pic);
                galleryImage.SaveAs(path);
                ObjProduct.ProductImage = (ImagePath.Replace("~", "..")) + pic;
            }
            if (ObjProduct.ProductId == 0)
                result = itemService.SaveProduct(ObjProduct);
            else
                result = itemService.UpdateProduct(ObjProduct);
            if (result)
            {
                return RedirectToAction("ManageProducts", new { });
            }
            return View();
        }
        [HttpPost]
        public ActionResult DeleteProduct(int ProductId)
        {
            bool result = false;string filepath = string.Empty;
            if (ProductId != 0)
                result = itemService.DeleteProduct(ProductId, UserAuthenticate.LogId,ref filepath);
            if (result)
            {
                try
                {
                    DeleteImageFiles(filepath);
                    return Json(new { Status = "Success", Message = "product deleted successful" });
                }
                catch (Exception ex)
                {
                    //LogWriter.LogWrite(ex.StackTrace);
                }
                return Json(new { Status = "Success", Message = "product deleted successful" });
            }
            return Json(new { Status = "Failure", Message = "Something went wrong" });
        }
        public void DeleteImageFiles(string fileName)
        {
            try
            {
                string path = Server.MapPath(fileName);
                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}