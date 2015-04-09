using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using IdentitySample.Models;
using Owin;
using System;
using Microsoft.Owin.Security.DataProtection;

namespace IdentitySample
{
    public partial class Startup
    {
        // Use a static provider, lest we receive the dreaded error:
        // The data protection operation was unsuccessful. 
        // This may have been caused by not having the user profile loaded for the current thread's user context, which may be the case when the thread is impersonating.
        public static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            DataProtectionProvider = app.GetDataProtectionProvider();
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        }
    }
}