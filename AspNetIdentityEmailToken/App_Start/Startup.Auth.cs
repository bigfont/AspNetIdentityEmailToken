// Comment this out of we're local
#define Azure

using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using IdentitySample.Models;

namespace IdentitySample
{
    public partial class Startup
    {
        // Use a static provider, lest we receive the dreaded error:
        // The data protection operation was unsuccessful. 
        // This may have been caused by not having the user profile loaded for the current thread's user context, which may be the case when the thread is impersonating.
        // What are the reasons NOT to use a static provider?
        public static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
#if Azure
            // where do we set the DataProtectionProvider that the app gets?
            DataProtectionProvider = app.GetDataProtectionProvider();
#else
           // DpapiDataProtectionProvider doesn't work in Azure because Azure is a web farm.
            DataProtectionProvider = new DpapiDataProtectionProvider("WebApp2015");
#endif

            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        }
    }
}