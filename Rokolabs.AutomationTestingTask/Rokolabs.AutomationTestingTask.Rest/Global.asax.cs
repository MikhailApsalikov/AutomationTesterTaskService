using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Rokolabs.AutomationTestingTask.Rest
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}