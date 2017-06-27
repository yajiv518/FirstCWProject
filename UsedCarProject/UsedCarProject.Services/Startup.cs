using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UsedCarProject.Services.Startup))]
namespace UsedCarProject.Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
