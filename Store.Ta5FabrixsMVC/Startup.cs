using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Store.Ta5FabrixsMVC.Startup))]
namespace Store.Ta5FabrixsMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
