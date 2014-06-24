using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using DataAccess.DataAccessLayer;
using DataAccess.Repository;
using DataAccessors;
using Web.Core;

namespace DeployHelper
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILoggingService _logger;

        protected void Application_Start()
        {
            LoadReferencedAssemblies();
            SetupDependencyInjection();
            //TODO: I'd really like to figure out an easy way to get rid of these two references to the data access project. Wrap this stuff in the Data Accessors project eventually?
            Database.SetInitializer(new DeployHelperInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //It feels like the dependency try is something you should only have to do once but it didn't like this
            SetupDependencyInjection();
            MappingHelper.SetupMaps();
        }

        private static void SetupDependencyInjection()
        {
            var builder = DependencyBuilder.SetupDependencyInjection();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<DeploymentController>().InstancePerHttpRequest();
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IRepository<>));
            builder.RegisterModelBinderProvider();
            builder.RegisterModule(new AutofacWebTypesModule());
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            DependencyResolver.Current.GetService<ILoggingService>();
            _logger = DependencyResolver.Current.GetService<ILoggingService>();
            _logger.SetLogger("DeployHelper");
            _logger.Info("Dependency injection setup finished.");
        }

        private static void LoadReferencedAssemblies()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();
            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\bin", "*.dll");
            var toLoad =
                referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            toLoad.ForEach(path => AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path)));
        }

        protected void Application_BeginRequest()
        {
        }
    }
}
