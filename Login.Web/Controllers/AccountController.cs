using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Login.Core.Data;
using Login.Core.OpenIdMVC;
using Login.Web.Code;
using Login.Web.Models;

namespace Login.Web.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor is used by the MVC framework to instantiate the controller using
        /// the default forms authentication and membership providers.
        /// </remarks>
        public AccountController()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="formsAuth">The forms authentication service.</param>
        /// <param name="service">The membership service.</param>
        /// <remarks>
        /// This constructor is not used by the MVC framework but is instead provided for ease
        /// of unit testing this type. See the comments at the end of this file for more
        /// information.
        /// </remarks>
        public AccountController(IFormsAuthentication formsAuth, IMembershipService service)
        {
            this.FormsAuth = formsAuth ?? new FormsAuthenticationService();
            this.MembershipService = service ?? new AccountMembershipService();//todo
        }

        public IFormsAuthentication FormsAuth { get; private set; }

        public IMembershipService MembershipService { get; private set; }

        [AllowAnonymous]
        public ActionResult LogOn()
        {
            return View();
        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(LogonViewModel model)
        {
            if (ModelState.IsValid)
            {
                Core.User user;
                if (UserDB.New().Validate(model.UserName, model.Password,out user))
                {

                    //this.FormsAuth.SignIn(model.UserName, model.RememberMe);//todo
                    HttpFormsAuthentication.SetAuthenticationCoolie(user,1);
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "账号密码错误");
                }
            }
            return View();
        }

        public ActionResult LogOff()
        {
            this.FormsAuth.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (filterContext.HttpContext.User.Identity is WindowsIdentity)
        //    {
        //        throw new InvalidOperationException("Windows authentication is not supported.");
        //    }
        //}
        

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string salt = null;
                var passwordHash = UserDB.GenerateHash(model.Password, ref salt);
                var user = new Login.Core.User
                {
                    DisplayName = model.DisplayName,
                    Username = model.UserName,
                    Email = model.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = salt,
                    Source = "self",
                    InsertDate = DateTime.Now,
                    InsertUserId = 1,
                    IsActive = 1
                };
                user.Insert();
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return  View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = UserDB.New().GetByUsername(model.UserName);
            ViewData["ResultMessage"] = "该用户不存在。";
            if (user != null)
            {
                ViewData["ResultMessage"] = "你密码已重置。";
                UserDB.New().ResetPassword(user.UserId, model.Password);
            }
            return RedirectToAction("ResetPasswordConfirmation", "Account");
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


    }
}