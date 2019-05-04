using Store.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Store.Ta5FabrixsMVC;

namespace Store.Ta5FabrixsMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /*if (!Context.Request.IsSecureConnection)
            {
                // This is an insecure connection, so redirect to the secure version
                UriBuilder uri = new UriBuilder(Context.Request.Url);
                uri.Scheme = "https";
                if (uri.Port > 32000 && uri.Host.Equals("localhost"))
                {
                    // Development box - set uri.Port to 44300 by default
                    uri.Port = 44300;
                }
                else
                {
                    uri.Port = 443;
                }

                Response.Redirect(uri.ToString());
            }*/
            System.Data.Entity.Database.SetInitializer(new StoreSeedData());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrapper.Run();
        }
    }
}
