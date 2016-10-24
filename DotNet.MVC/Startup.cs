using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DotNet.MVC.Startup))]
//[assembly: log4net.Config.XmlConfigurator(ConfigFile ="Web.config", Watch = true)]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "~/Config/Log4NetConfig.xml", Watch = true)]
namespace DotNet.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
