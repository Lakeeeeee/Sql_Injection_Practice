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
            // �e���إߪ�
            ContainerBuilder builder = new ContainerBuilder();

            // ���UControllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // ���U�A��
            builder.RegisterType<SqlConnectionFactory>().As<ISqlConnectionFactory>().AsImplementedInterfaces();

            // �إ߮e��
            Autofac.IContainer container = builder.Build();

            // ���w�ѪR��
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
