using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Wine.Core.DataAccess;

namespace Wine.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var db = new WineDbContext();
            db.Database.CreateIfNotExists();

            db.Dispose();
        }
    }
}
