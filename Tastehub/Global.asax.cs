using Stripe;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tastehub
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["SecretKey"]);
        }
    }
}
