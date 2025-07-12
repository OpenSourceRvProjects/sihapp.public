using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sihapp.WebProject.Startup))]
namespace Sihapp.WebProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
