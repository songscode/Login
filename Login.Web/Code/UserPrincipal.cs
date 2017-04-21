using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Login.Web.Code
{
    public class UserPrincipal: IPrincipal
    {
        public UserPrincipal(FormsAuthenticationTicket ticket, Core.User account)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");
            if (account == null)
                throw new ArgumentNullException("User");

            this.Identity = new FormsIdentity(ticket);
            this.Account = account;
        }

        public bool IsInRole(string role)
        {
            if (string.IsNullOrEmpty(role))
                return true;
            if (this.Account == null)
                return false;
            return true;
        }

        public Core.User Account { get; set; }

        public IIdentity Identity { get; private set; }
    }
}