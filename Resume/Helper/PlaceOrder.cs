using CarisBrook.Service.UserService;
using DemoModel.ViewModel;
using DemoService.User;
using ResturantBooking.Controllers;
using ResturantBooking.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;

namespace IndianLotus.Helper
{
    public class PlaceOrder
    {
        
        public static void UpdateOrder(string filePath ,bool result, ref decimal totalPrice, int AddressId, ref int shippingResponseId, string PaymentMode)
        {
            UserService _userService = new UserService();
            HomeService homeService = new HomeService();
            var user = _userService.GetUsersDetailsById(long.Parse(UserAuthenticate.LogId));
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
            LogWriter.LogWrite("Email Sending start", filePath);
            EmailNotification.SendMailConfirmation(user.Email,
                UserAuthenticate.UserName, "Order Confirmation", mailBody, filePath);
            LogWriter.LogWrite("Email Sending End", filePath);
        }
    }
}