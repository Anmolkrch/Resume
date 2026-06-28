using TastehubModel.ViewModel;
using TastehubService.Service.ItemService;
using TastehubService.User;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Tastehub.Service.UserService;
using Tastehub.Web.Helper;
using Utility;

namespace IndianLotus.Controllers
{
    public class StripeController : Controller
    {
        // GET: Stripe
        UserService _userService = new UserService();
        ItemService _itemService = new ItemService();
        HomeService homeService = new HomeService();
        [CustomAuthorize]
        public ActionResult Charge(string stripeEmail, string stripeToken)
        {
            var customers = new CustomerService();
            double dollar_amount;
            int cents;
            List<CartSpiceModel> cartSpiceModel = new List<CartSpiceModel>();
            var charges = new ChargeService();
            string filePath = string.Empty; bool result = true;
            decimal totalPrice = 0; int shippingResponseId = 0, AddressId = 0;
            string currency = ConfigurationManager.AppSettings["currency"].ToString();
            filePath = Server.MapPath(Url.Content(ConfigurationManager.AppSettings["logfilePath"].ToString()));
            dollar_amount = Decimal.ToDouble(homeService.GetOrderAmount(long.Parse(UserAuthenticate.LogId), cartSpiceModel));
            try
            {
                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Source = stripeToken
                });
                cents = (int)(dollar_amount * 100);
                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = Convert.ToInt64(cents),
                    Description = "Food Order Charge",
                    Currency = currency,
                    Customer = customer.Id
                });
                LogWriter.LogWrite(charge.ToString(), filePath);
                try
                {

                    UpdateOrder(filePath, result, ref totalPrice, AddressId, ref shippingResponseId, "Card");
                    if (charge.Status == "succeeded")
                    {
                        homeService.PaymentDetails(long.Parse(UserAuthenticate.LogId),totalPrice,shippingResponseId, charge.StripeResponse.Content.ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.LogWrite(ex.Message, filePath);
                }
                LogWriter.LogWrite(charge.ToString(), filePath);
            }

            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.Message, filePath);
            }
            return RedirectToAction("MyOrder", "Home");
        }
        private void UpdateOrder(string filePath, bool result, ref decimal totalPrice, int AddressId, ref int shippingResponseId, string PaymentMode)
        {
            var user = _userService.GetUsersDetailsById(long.Parse(UserAuthenticate.LogId));
            if (AddressId==0)
            {
                AddressId = user.UserDetail.Id;
            }
            List<CartViewModel> cartList = homeService.PlaceCartOrder(long.Parse(UserAuthenticate.LogId),
                ref result, ref totalPrice, AddressId, ref shippingResponseId, PaymentMode);
            string mailBody = string.Empty;
            mailBody = CustomUtil.RenderRazorViewToString(this, "_OrderConfirmation", cartList);
            mailBody = mailBody.Replace("$HomeUrl", System.Configuration.ConfigurationManager.AppSettings["HomeUrl"]);
            mailBody = mailBody.Replace("$RECIPIENTNAME", UserAuthenticate.UserName);
            mailBody = mailBody.Replace("$total", totalPrice.ToString());
            mailBody = mailBody.Replace("$UserDeliveryAddress", user.UserDetail.CurAddress1 + user.UserDetail.CurAddress2);
            mailBody = mailBody.Replace("$UserPhone", user.UserDetail.ProfileUrl);
            mailBody = mailBody.Replace("$UserEmail", user.Email);
            mailBody = mailBody.Replace("$PinCode", user.UserDetail.CurZipCode);
            mailBody = mailBody.Replace("$PaymentMode", "Card");
            LogWriter.LogWrite("Email Sending start", filePath);
            EmailNotification.SendMailConfirmation(user.Email,
                UserAuthenticate.UserName, "Order Confirmation", mailBody, filePath);
            LogWriter.LogWrite("Email Sending End", filePath);
        }

    }
}