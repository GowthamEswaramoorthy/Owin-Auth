using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using Owin.Security.Providers.Google;

[assembly: OwinStartup(typeof(Owin_Auth.App_Start.Startup))]

namespace Owin_Auth.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Comment/AddComment")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Add the Google console Client ID and Client Secret
            var options = new GoogleAuthenticationOptions
            {
                ClientId = "",
                ClientSecret = ""
            };

            //options.Scope.Add("identify");
            app.UseGoogleAuthentication(options);
        }
    }
}
