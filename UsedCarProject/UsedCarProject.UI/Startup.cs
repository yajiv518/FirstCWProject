using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UsedCarProject.UI.Startup))]
namespace UsedCarProject.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
