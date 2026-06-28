using TastehubService.Service.ItemService;
using PagedList;
using System.Web.Mvc;

namespace Tastehub.Controllers
{
    public class OrderController : Controller
    {
        ItemService itemService = new ItemService();
        public ActionResult ManageOrders(int? pageSize, int? page)
        {

            int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);
            ViewBag.PageSize = pageDataSize;
            var product = itemService.GetAllOrderList().ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Orders", product) : View(product);

        }
        public ActionResult ViewOrders(int ShippingId)
        {
            var product = itemService.GetOrderDetail(ShippingId);
            return (ActionResult)PartialView("_ViewOrders", product);

        }
    }
}