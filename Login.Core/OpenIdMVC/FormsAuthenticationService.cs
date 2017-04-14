using System;
using System.Web;
using System.Web.Security;

namespace Login.Core.OpenIdMVC {
    public class FormsAuthenticationService : IFormsAuthentication {
		public string SignedInUsername {
			get { return HttpContext.Current.User.Identity.Name; }
		}

		public DateTime? SignedInTimestampUtc {
			get {
				var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
				if (cookie != null) {
					var ticket = FormsAuthentication.Decrypt(cookie.Value);
					return ticket.IssueDate.ToUniversalTime();
				} else {
					return null;
				}
			}
		}

		public void SignIn(string userName, bool createPersistentCookie) {
			FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
		}

		public void SignOut() {
			FormsAuthentication.SignOut();
		}
	}
}
