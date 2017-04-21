using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Login.Web.Code
{
    public class FormAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 先进入此方法，此方法中会调用 AuthorizeCore 验证逻辑，验证不通过会调用 HandleUnauthorizedRequest 方法
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User as UserPrincipal;
            if (user != null)
                return true;
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //验证不通过，直接跳转到相应页面，注意：如果不是哟娜那个以下跳转，则会继续执行Action方法
            //filterContext.Result = new RedirectResult("~/Login/Index");
        }
    }
}