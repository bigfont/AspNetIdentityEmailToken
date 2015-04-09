using Microsoft.Owin.Security.DataProtection;
using Owin;

namespace IdentitySample
{
    public partial class Startup
    {
        // Use a static provider, lest we receive the dreaded error:
        // The data protection operation was unsuccessful. 
        // This may have been caused by not having the user profile loaded for the current thread's user context, which may be the case when the thread is impersonating.
        public static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            DataProtectionProvider = new DpapiDataProtectionProvider("WebApp2015");
            ConfigureAuth(app);
        }
    }
}
