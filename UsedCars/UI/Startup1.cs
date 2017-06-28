using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UI.Startup1))]
namespace UI
{
    public partial class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
