using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Owin_Auth.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddComment()
        {
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            if (!Request.IsAuthenticated)
            {
                return new ChallengeResult("Google",
                    Url.Action("ExternalLoginCallback", "Comment", new { ReturnUrl = returnUrl }));
            }
            else
            {
                //Method to do buisness logic
            }

            return Redirect("/Home/Secure");
        }

        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = HttpContext.GetOwinContext().Authentication.GetExternalLoginInfo();
            if (loginInfo.ExternalIdentity.IsAuthenticated)
            {
                SignIn();
                return Redirect(returnUrl);
            }

            return new HttpUnauthorizedResult();
        }

        private void SignIn()
        {
            var loginInfo = HttpContext.GetOwinContext().Authentication.GetExternalLoginInfo();
            if (loginInfo.ExternalIdentity.IsAuthenticated)
            {
                var manager = HttpContext.GetOwinContext().Authentication;
                var claims = loginInfo.ExternalIdentity.Claims;
                manager.SignIn(new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.Now.AddMinutes(60)
                },
                    new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie));
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
            }

            private string LoginProvider { get; }
            private string RedirectUri { get; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}