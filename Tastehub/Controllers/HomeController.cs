using TastehubModel.ViewModel;
using TastehubService.MenuNamespace;
using TastehubService.Service.ItemService;
using TastehubService.User;
using IndianLotus.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Tastehub.Service.BookingService;
using Tastehub.Service.UserService;
using Tastehub.Utility.Helper;
using Tastehub.Web.Helper;
using Utility;

namespace Tastehub.Controllers
{
    
    public class HomeController : Controller
    {
        BookingService _bookingService = new BookingService();
        MenuService _MenuService = new MenuService();
        UserService _userService = new UserService();
        ItemService _itemService = new ItemService();
        HomeService homeService = new HomeService();
        
        public ActionResult Index()

        {
            List<GalleryViewModel> gallery = new List<GalleryViewModel>();
            gallery = _MenuService.GetAllGallery().Where(x => x.TypeId == 101 && x.IsActive == true).ToList();
            ViewBag.VideoURL = ConfigurationManager.AppSettings["VideoURL"].ToString();
            ViewBag.Category = _itemService.GetCategoryList();
            var item = _itemService.GetItemList(0);
            ViewBag.ItemList1 = item.Where(x => x.CategoryId == 1).ToList();
            ViewBag.ItemList2 = item.Where(x => x.CategoryId == 2);
            ViewBag.ItemList3 = item.Where(x => x.CategoryId == 3);
            ViewBag.ItemList4 = item.Where(x => x.CategoryId == 4);
            ViewBag.ItemList5 = item.Where(x => x.CategoryId == 5);
            ViewBag.AllItems = item.ToList();
            return View(gallery);
        }
        public ActionResult contact()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Gallery()
        {
            List<GalleryViewModel> gallery = new List<GalleryViewModel>();
            gallery = _MenuService.GetAllGallery().Where(x => x.TypeId == 101 && x.IsActive == true).ToList();
            return View(gallery);
        }
        [HttpPost]
        public ActionResult BookingRequest(BookingReqestViewModel bookingReqestViewModel)
        {
            try
            {
                string mailBody = string.Empty, filePath = string.Empty;
                filePath = Server.MapPath(Url.Content(ConfigurationManager.AppSettings["logfilePath"].ToString()));
                bool result = _bookingService.SaveBooking(bookingReqestViewModel);
                mailBody = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplates/BookingConfirmation.html"));//RenderPartialToString("_Notify", "", ControllerContext);
                mailBody = mailBody.Replace("$HomeUrl", System.Configuration.ConfigurationManager.AppSettings["HomeUrl"]);
                mailBody = mailBody.Replace("$RECIPIENTNAME", bookingReqestViewModel.Name);
                mailBody = mailBody.Replace("$UserPhone", bookingReqestViewModel.PhoneNo);
                mailBody = mailBody.Replace("$UserEmail", bookingReqestViewModel.Email);
                mailBody = mailBody.Replace("$Date", bookingReqestViewModel.Date.ToShortDateString());
                mailBody = mailBody.Replace("$Time", bookingReqestViewModel.Time.ToString());
                mailBody = mailBody.Replace("$People", bookingReqestViewModel.People.ToString());
                LogWriter.LogWrite("Email Sending start", filePath);
                EmailNotification.SendMailConfirmation(bookingReqestViewModel.Email, bookingReqestViewModel.Name, "Booking Confirmation", mailBody, filePath);
                LogWriter.LogWrite("Email Sending End", filePath);
                if (result)
                    return Json(new { Status = "Success", Message = "Thanks for booking with us .Shortly you will get confirmation" });
                else
                    return Json(new { Status = "Failure", Message = "oop something went wrong" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Failure", Message = ex.Message });
            }
        }
        public ActionResult Menu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactViewModel contactObj)
        {
            try
            {
                string mailBody = string.Empty, filePath = string.Empty;
                filePath = Server.MapPath(Url.Content(ConfigurationManager.AppSettings["logfilePath"].ToString()));
                bool result = _bookingService.SaveContact(contactObj);
                mailBody = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplates/ContactUsTemplate.html"));
                mailBody = mailBody.Replace("$HomeUrl", System.Configuration.ConfigurationManager.AppSettings["HomeUrl"]);
                mailBody = mailBody.Replace("$RECIPIENTNAME", contactObj.Name);
                mailBody = mailBody.Replace("$Name", contactObj.Name);
                mailBody = mailBody.Replace("$Phone", contactObj.PhoneNo);
                mailBody = mailBody.Replace("$Email", contactObj.Email);
                mailBody = mailBody.Replace("$Message", contactObj.Message);
                LogWriter.LogWrite("Email Sending start", filePath);
                EmailNotification.SendMailConfirmation(contactObj.Email, contactObj.Name, "Contact Email", mailBody, filePath);
                LogWriter.LogWrite("Email Sending End", filePath);
                if (result)
                    return Json(new { Status = "Success", Message = "Thanks for contacting with us .Shortly you will get confirmation" });
                else
                    return Json(new { Status = "Failure", Message = "oop something went wrong" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Failure", Message = ex.Message });
            }
        }
        public ActionResult Item()
        {
            ItemService itemService = new ItemService();
            return View(itemService.GetItemList(0));
        }
        public ActionResult ItemByCategory(int CategoryId)
        {
            ItemService itemService = new ItemService();
            return View("Item",itemService.GetItemList(CategoryId));
        }
        public ActionResult ItemDetail(int Id)
        {
            return View(_itemService.GetItemList(0).Where(x => x.ProductId == Id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult AddItemCart(int UserId,int ItemId)
        {
            bool result = true;
            int count = 0;
            try
            {
                result= homeService.AddItemInCart(UserId, ItemId, ref count);
                if (result)
                    return Json(new { Status = "Success", Message = "Added Item in cart" , count });
                else
                    return Json(new { Status = "Failure", Message = "oop something went wrong" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Failure", Message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
                try
                {
                    loginViewModel.PasswordHash = SecurityHelper.CreatePasswordHash(loginViewModel.Password, "");
                    UserViewModel authenticatedUser = null;
                    authenticatedUser = _userService.LoginAuthentication(loginViewModel);

                    if (authenticatedUser != null)
                    {
                        string rememberme = (loginViewModel.RememberMe) ? "true" : "false";
                        UserAuthenticate.AddLoginCookie(authenticatedUser.FirstName + " " + authenticatedUser.LastName,
                         authenticatedUser.UserType.Name, authenticatedUser.Id.ToString(),
                                  authenticatedUser.UserType.Code, rememberme, authenticatedUser.UserTypeId);
                    return Json(new { Status = "Success", Message = "Login attempt was successful",Role=authenticatedUser.UserType.Name });
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Not Authenticated ");
                        return Json(new { Status = "Failure", Message = "Invalid user details. kindly use valid credentials" });
                    }
                        
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                    return Json(new { Status = "Failure", Message = ex.Message });
                }


        }
        [CustomAuthorize]
        public ActionResult MyCart()
        {
            List<CartViewModel> cartViewModel =new List<CartViewModel>();
            if (UserAuthenticate.IsAuthenticated)
            {
                try
                {
                    cartViewModel = homeService.GetMyCart(int.Parse(UserAuthenticate.LogId));
                }
                catch (Exception ex)
                {
                    return Json(new { Status = "Failure", Message = ex.InnerException.Message });
                }
            }
            return View(cartViewModel);
        }
        [CustomAuthorize]
        public ActionResult Order()
        {
            List<UserDetailViewModel> userDetails = new List<UserDetailViewModel>();
            List<CartSpiceModel> cartSpiceModel = new List<CartSpiceModel>();
            var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            var publishableKey = ConfigurationManager.AppSettings["PublishableKey"];
            ViewBag.secretKey = secretKey;
            ViewBag.StripePublishKey = publishableKey;
            ViewBag.TotalAmount = homeService.GetOrderAmount(long.Parse(UserAuthenticate.LogId), cartSpiceModel);
            ViewBag.City = _MenuService.GetCity(true);
            try
            {
                userDetails = homeService.GetMyAddress(long.Parse(UserAuthenticate.LogId));
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Failure", Message = ex.InnerException.Message });
            }
            return View(userDetails);
        }
        [CustomAuthorize]
        [HttpPost]
        public ActionResult OrderNow(List<CartSpiceModel> cartSpiceModel)
        {
            try
            {
                homeService.GetOrderAmount(long.Parse(UserAuthenticate.LogId), cartSpiceModel);
                return Json(new { Status = "Success", Message = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Failure", Message = ex.InnerException.Message });
            }
        }
        [CustomAuthorize]
        public ActionResult PlaceOrder(int AddressId,string PaymentType)
        {
            bool result = true;
            decimal totalPrice = 0; int shippingResponseId=0;
            try
            {
                string filePath = Server.MapPath(Url.Content(ConfigurationManager.AppSettings["logfilePath"].ToString()));
                UpdateOrder(filePath,result, ref totalPrice, AddressId,ref shippingResponseId, PaymentType);
                if (result)
                    return Json(new { Status = "Success", Message = "Great your order has been place.You will get notification soon" });
                else
                    return Json(new { Status = "Failure", Message = "oop something went wrong" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Failure", Message = ex.Message });
            }
        }
        [CustomAuthorize]
        public ActionResult MyAddresses()
        {
            List<UserDetailViewModel> userDetails = new List<UserDetailViewModel>();

            try
            {
                HomeService homeService = new HomeService();
                userDetails = homeService.GetMyAddress(long.Parse(UserAuthenticate.LogId));
                ViewBag.City = _MenuService.GetCity(true);
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Failure", Message = ex.InnerException.Message });
            }
            return View(userDetails);
        }
        [CustomAuthorize]
        public ActionResult MyOrder()
        {
            List<CartViewModel> cartViewModel = new List<CartViewModel>();
            if (UserAuthenticate.IsAuthenticated)
            {
                try
                {
                    HomeService homeService = new HomeService();
                    cartViewModel = homeService.GetMyOrders(int.Parse(UserAuthenticate.LogId));
                }
                catch (Exception ex)
                {
                    return Json(new { Status = "Failure", Message = ex.InnerException.Message });
                }
            }
            return View(cartViewModel);
        }
        [CustomAuthorize]
        public ActionResult SaveMyAdress(UserDetailViewModel userDetailViewModel)
        {
            bool result = true;
            try
            {
                HomeService homeService = new HomeService();
                result = homeService.SaveMyAddress(long.Parse(UserAuthenticate.LogId), userDetailViewModel);
                if (result)
                    return Json(new { Status = "Success", Message = "Great you address has been save" });
                else
                    return Json(new { Status = "Failure", Message = "oop something went wrong" });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Failure", Message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult RemoveItemCart(int UserId, int ItemId)
        {
            bool result = true;
            int count = 0;
            try
            {
                HomeService homeService = new HomeService();
                result = homeService.RemoveItemInCart(UserId, ItemId,ref count);
                if (result)
                    return Json(new { Status = "Success", Message = "Item removed from your cart" , count });
                else
                    return Json(new { Status = "Failure", Message = "oop something went wrong", count });
            }
            catch (Exception ex)
            {
                return Json(new { Status = "Failure", Message = ex.Message });
            }
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "USD",
                price = "1",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "3", // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "your generated invoice number", //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
        private void UpdateOrder(string filePath,bool result, ref decimal totalPrice,int AddressId, ref int shippingResponseId,string PaymentMode)
        {
            var user = _userService.GetUsersDetailsById(long.Parse(UserAuthenticate.LogId));
            List<CartViewModel> cartList = homeService.PlaceCartOrder(long.Parse(UserAuthenticate.LogId),
                ref result, ref totalPrice, AddressId,ref shippingResponseId, PaymentMode);
            string mailBody = string.Empty;
            mailBody = CustomUtil.RenderRazorViewToString(this, "_OrderConfirmation", cartList);
            mailBody = mailBody.Replace("$HomeUrl", System.Configuration.ConfigurationManager.AppSettings["HomeUrl"]);
            mailBody = mailBody.Replace("$RECIPIENTNAME", UserAuthenticate.UserName);
            mailBody = mailBody.Replace("$total", totalPrice.ToString());
            mailBody = mailBody.Replace("$UserDeliveryAddress", user.UserDetail.CurAddress1 + user.UserDetail.CurAddress2);
            mailBody = mailBody.Replace("$UserPhone", user.UserDetail.ProfileUrl);
            mailBody = mailBody.Replace("$UserEmail", user.Email);
            mailBody = mailBody.Replace("$PinCode", user.UserDetail.CurZipCode);
            mailBody = mailBody.Replace("$PaymentMode", PaymentMode);
            //LogWriter.LogWrite("Email Sending start", filePath);
            EmailNotification.SendMailConfirmation(user.Email,
                UserAuthenticate.UserName, "Order Confirmation", mailBody, filePath);
            //LogWriter.LogWrite("Email Sending End", filePath);
        }
    }
}