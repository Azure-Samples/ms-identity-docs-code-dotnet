using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace WebApp_SAML_to_OIDC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
