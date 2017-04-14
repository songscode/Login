using System;
using System.Web;

namespace Login.Core.OpenIdMVC {
    public  static class Util {
		public  static Uri GetAppPathRootedUri(string value) {
			string appPath = HttpContext.Current.Request.ApplicationPath.ToLowerInvariant();
			if (!appPath.EndsWith("/")) {
				appPath += "/";
			}

			return new Uri(HttpContext.Current.Request.Url, appPath + value);
		}
	}
}
