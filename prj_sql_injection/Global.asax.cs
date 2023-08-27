using Autofac;
using Autofac.Integration.Mvc;
using prj_sql_injection.App_Start;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace prj_sql_injection
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutofacConfig.Register();

            var sqlListener = new TextWriterTraceListener("sqlLog.txt");
            Trace.Listeners.Add(sqlListener);
            Trace.AutoFlush = true;
        }
    }

    public static class AutofacConfig
    {
        public static void Register()
        {
            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            // 註冊Controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // 註冊服務
            builder.RegisterType<SqlConnectionFactory>().As<ISqlConnectionFactory>().AsImplementedInterfaces();

            // 建立容器
            Autofac.IContainer container = builder.Build();

            // 指定解析器
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
