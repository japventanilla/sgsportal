using SGS.Business.Controllers;
using SGS.Common.Controllers;
using SGS.Portal.Web.Classes;
using SGS.Portal.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SGS.Portal.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        SessionContext context = new SessionContext();
        private static string appName = ConfigurationManager.AppSettings["AppName"].ToString();

        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            if (UserHelper.CurrentUser == null)
            {
                LogOnViewModel model = new LogOnViewModel();
                TempData["ReturnUrl"] = ReturnUrl;

                if (Request.Cookies[appName] != null)
                    model.EmployeeId = Request.Cookies[appName].Values["EmployeeId"];

                return View(model);
            }
            else
            {
                return Redirect("~/");
            }
        }

        [HttpPost]
        public ActionResult Login(LogOnViewModel model)
        {
            if (ModelState.IsValid)
            {
                SGS.Data.EntityFrameworks.User authenticatedUser = UserManager.Instance.Get(model.EmployeeId, model.Password);
                if (authenticatedUser != null)
                {

                    if (model.IsRemember)
                    {
                        HttpCookie cookie = new HttpCookie(appName);
                        cookie.Values.Add("EmployeeId", model.EmployeeId);
                        cookie.Expires = DateHelper.DateTimeNow.AddDays(15);
                        Response.Cookies.Add(cookie);
                    }

                    //MemberManager.Instance.SaveMemberLastLoggedOn(authenticatedUser.MemberId);
                    Session["CurrentUser"] = authenticatedUser;
                    context.SetAuthenticationToken(authenticatedUser.UserId.ToString(), false, authenticatedUser);

                    //additional startup values
                    string[] roles = Roles.GetRolesForUser(authenticatedUser.UserId.ToString());

                    //if (TempData["ReturnUrl"] != null)
                    //    return Redirect("~/Payroll"); 
                    //else
                    //    return Redirect("~/Payroll"); 

                    if (TempData["ReturnUrl"] != null)
                        return Redirect(HttpUtility.UrlDecode(TempData["ReturnUrl"].ToString()));
                    else
                        return Redirect("~/");

                }
                else
                {
                    ViewBag.IsError = true;
                }
            }
            else
            {
                ViewBag.ValidationError = true;
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Remove("CurrentUser");
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        
        public ActionResult UnderMaintenance()
        {
            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

    }
}
