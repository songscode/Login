using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace Login.Web.Code
{
    public class HttpFormsAuthentication
    {
        //将用户信息通过ticket加密保存到cookie
        public static void SetAuthenticationCoolie(Core.User account, int rememberDay = 0)
        {
            if (account == null)
                throw new ArgumentNullException("account");

            //序列化account对象
            string accountJson = JsonConvert.SerializeObject(account);
            //创建用户票据
            var ticket = new FormsAuthenticationTicket(1, account.Username, DateTime.Now, DateTime.Now.AddDays(rememberDay), false, accountJson);
            //加密
            string encryptAccount = FormsAuthentication.Encrypt(ticket);

            //创建cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptAccount)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Domain = FormsAuthentication.CookieDomain,
                Path = FormsAuthentication.FormsCookiePath
            };

            if (rememberDay > 0)
                cookie.Expires = DateTime.Now.AddDays(rememberDay);

            //写入Cookie
            HttpContext.Current.Response.Cookies.Remove(cookie.Name);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        //获取cookie并解析出用户信息
        public static UserPrincipal TryParsePrincipal(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            HttpRequest request = context.Request;
            HttpCookie cookie = request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                return null;
            }
            //解密coolie值
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            if (ticket==null||string.IsNullOrEmpty(ticket.UserData))
            {
                return null;
            }
            Core.User account = JsonConvert.DeserializeObject<Core.User>(ticket.UserData);
            return new UserPrincipal(ticket, account);
        }
    }
}