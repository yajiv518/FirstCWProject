using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Services.Startup))]
namespace Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
