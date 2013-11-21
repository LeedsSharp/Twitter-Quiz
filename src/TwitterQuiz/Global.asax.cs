using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using SimpleAuthentication.Core;
using SimpleAuthentication.Mvc;
using SimpleAuthentication.Mvc.Caching;
using TwitterQuiz.Controllers;

namespace TwitterQuiz
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            builder.RegisterType<AuthenticationCallbackProvider>().As<IAuthenticationCallbackProvider>();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterControllers(typeof(SimpleAuthenticationController).Assembly);
            builder.RegisterType<CookieCache>().As<ICache>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}