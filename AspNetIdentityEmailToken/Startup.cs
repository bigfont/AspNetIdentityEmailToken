using Microsoft.Owin.Security.DataProtection;
using Owin;

namespace IdentitySample
{
    public partial class Startup
    {
        public static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            DataProtectionProvider = new DpapiDataProtectionProvider("WebApp2015");
            ConfigureAuth(app);
        }
    }
}
