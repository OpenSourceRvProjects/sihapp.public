using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sihapp.InfoWeb.Startup))]
namespace Sihapp.InfoWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
