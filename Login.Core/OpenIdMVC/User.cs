using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Login.Core.OpenIdMVC;

namespace Login.Core
{
    public partial class User
    {
        public static Uri ClaimedIdentifierBaseUri
        {
            get { return Util.GetAppPathRootedUri("user/"); }
        }

        public  static Uri GetClaimedIdentifierForUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            return new Uri(ClaimedIdentifierBaseUri, username.ToLowerInvariant());
        }

        public static string GetUserFromClaimedIdentifier(Uri claimedIdentifier)
        {
            Regex regex = new Regex(@"/user/([^/\?]+)");
            Match m = regex.Match(claimedIdentifier.AbsoluteUri);
            if (!m.Success)
            {
                throw new ArgumentException();
            }

            return m.Groups[1].Value;
        }

        public  static Uri GetNormalizedClaimedIdentifier(Uri uri)
        {
            return GetClaimedIdentifierForUser(GetUserFromClaimedIdentifier(uri));
        }
    }
}
