using TastehubModel.ViewModel;
using System;
using System.Web.Mvc;
using Tastehub.Service.UserService;
using Tastehub.Utility.Helper;
using Tastehub.Web.Helper;
using static Utility.Enums;

namespace Tastehub.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {

        UserService _userService = new UserService();

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (UserAuthenticate.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Request.Cookies["ES"].Expires = DateTime.Now.AddHours(24);
                if (UserAuthenticate.UserTypeId== (int)UserType.User)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                return View();
            }
            //if (TempData["result"] != null)
            //{
            //    var myJsonResult = TempData["result"] as MyJsonResult;
            //    if (myJsonResult.data != null)
            //        return View(new LoginViewModel { Email = myJsonResult.data.ToString() });
            //}
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.PasswordHash = SecurityHelper.CreatePasswordHash(model.Password, "");

                    UserViewModel authenticatedUser = null;
                    authenticatedUser = _userService.LoginAuthentication(model);

                    if (authenticatedUser != null)
                    {
                        string rememberme = (model.RememberMe) ? "true" : "false";
                        UserAuthenticate.AddLoginCookie(authenticatedUser.FirstName + " " + authenticatedUser.LastName, 
                            authenticatedUser.UserType.Name, authenticatedUser.Id.ToString(),
                                     authenticatedUser.UserType.Code, rememberme,authenticatedUser.UserTypeId);
                        return RedirectToAction("Dashboard", "Admin");

                    }
                    else
                    {
                        ModelState.AddModelError("", "User Not Authenticated ");
                        // ViewBag.ErrorMsg = "Please check your username and password! ";
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                }

            }
            return View(model);
        }
        
        public ActionResult LogOff()
        {
            TempData.Keep("result");
            UserAuthenticate.Logout(System.Web.HttpContext.Current);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Unauthorized()
        {
            UserAuthenticate.Logout(System.Web.HttpContext.Current);
            return View();
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Register(string returnUrl)
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserViewModel model)
        {
            if (_userService.IsUserExists(model.Email, model.Id.ToString()))
                ModelState.AddModelError("Email", "Email is already taken by someone.");
            if (ModelState.IsValid)
            {
                try
                {
                    model.PasswordHash = SecurityHelper.CreatePasswordHash(model.Password, "");
                    var user = _userService.RegisterUsers(model);

                    if (user != null)
                    {
                        UserAuthenticate.AddLoginCookie(user.FirstName + " " + user.LastName,
                            user.UserType.Name, user.Id.ToString(),
                                     user.UserType.Code, "", user.UserTypeId);
                        return RedirectToAction("Item", "Home");
                    }
                    else
                    {
                        //ModelState.Add(model.er)
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                }

            }
            return View(model);
        }
        [AllowAnonymous]
        public JsonResult IsUserEmailExists(string Email, string Id)
        {
            return Json(!_userService.IsUserExists(Email, Id), JsonRequestBehavior.AllowGet);
        }
    }
}