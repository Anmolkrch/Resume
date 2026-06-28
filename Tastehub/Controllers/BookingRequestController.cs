using PagedList;
using System.Web.Mvc;
using Tastehub.Service.BookingService;
using Tastehub.Web.Helper;
using Utility;

namespace Tastehub.Controllers
{
    [CustomAuthorize]
    [RoleAuthorize(AppConstant.RoleAdmin, AppConstant.RoleSuperAdmin)]
    public class BookingRequestController : Controller
    {
        // GET: BookingRequest
        BookingService bookingService = new BookingService();
 
        public ActionResult ManageBookings(int? pageSize, int? page)
        {

            int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);
            ViewBag.PageSize = pageDataSize;
            var booking = bookingService.GetBookingList().ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Bookings", booking) : View(booking);

        }
        public ActionResult ManageContactRequests(int? pageSize, int? page)
        {

            int pageDataSize = (pageSize ?? 10);
            int pageNumber = (page ?? 1);
            ViewBag.PageSize = pageDataSize;
            var booking = bookingService.GetContactRequestList().ToPagedList(pageNumber, pageDataSize);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_ContactRequests", booking) : View(booking);

        }
    }
}