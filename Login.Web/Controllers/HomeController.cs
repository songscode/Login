using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;

namespace Login.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// The OAuth 2.0 client object to use to obtain authorization and authorize outgoing HTTP requests.
        /// </summary>
        private  WebServerClient Client;

        /// <summary>
        /// The details about the sample OAuth-enabled WCF service that this sample client calls into.
        /// </summary>
        private  AuthorizationServerDescription authServerDescription = new AuthorizationServerDescription
        {
            TokenEndpoint = new Uri("http://localhost:23442/OAuth/Token"),
            AuthorizationEndpoint = new Uri("http://localhost:23442/OAuth/Authorize"),
        };

        public HomeController()
        {
            Client = new WebServerClient(authServerDescription, "sampleconsumer", "samplesecret");
        }
        private  IAuthorizationState Authorization
        {
            get { return (AuthorizationState)Session["Authorization"]; }
            set {Session["Authorization"] = value; }
        }
        public ActionResult Index()
        {
            return View();
        }
        

        public async Task<ActionResult> About()
        {

            // Check to see if we're receiving a end user authorization response.
            var authorization =await Client.ProcessUserAuthorizationAsync(Request, Response.ClientDisconnectedToken);
            if (authorization != null)
            {
                // We are receiving an authorization response.  Store it and associate it with this user.
                Authorization = authorization;
                Response.Redirect(Request.Path); // get rid of the /?code= parameter
            }
            string msg = "no Authorization";

            if (Authorization != null)
            {
                // Indicate to the user that we have already obtained authorization on some of these.
                msg = "Authorization received!";
                if (Authorization.AccessTokenExpirationUtc.HasValue)
                {
                    TimeSpan timeLeft = Authorization.AccessTokenExpirationUtc.Value - DateTime.UtcNow;
                    msg += string.Format(CultureInfo.CurrentCulture, "  (access token expires in {0} minutes)", Math.Round(timeLeft.TotalMinutes, 1));
                }
            }

            ViewBag.Message = msg;

            return View();
        }

        public async Task<ActionResult> GetAuth()
        {
            var request =await Client.PrepareRequestUserAuthorizationAsync(null, cancellationToken: Response.ClientDisconnectedToken);
            await request.SendAsync();
            return new EmptyResult();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}