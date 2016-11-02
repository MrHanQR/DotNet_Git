using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DotNet.Web.Startup))]
namespace DotNet.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
