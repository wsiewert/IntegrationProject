using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IntegrationProject.Startup))]
namespace IntegrationProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
