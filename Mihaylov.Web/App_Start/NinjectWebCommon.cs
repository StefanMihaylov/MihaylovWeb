[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Mihaylov.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Mihaylov.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Mihaylov.Web.App_Start
{
    using System;
    using System.Web;
    using System.Web.Hosting;
    using GoogleDrive;
    using GoogleDrive.Interfaces;
    using log4net.Config;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using log4net;
    using log4net.Appender;
    using log4net.Layout;
    using log4net.Repository.Hierarchy;

    using Ninject;
    using Ninject.Extensions.Logging.Log4net;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
           // XmlConfigurator.Configure();
           // var settings = new NinjectSettings { LoadExtensions = false };
           // var kernel = new StandardKernel(settings, new [] { new Log4NetModule() });

            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void Log4NetSetup(string filePath)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = true;
            roller.File = filePath;
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "10MB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = log4net.Core.Level.Info;
            hierarchy.Configured = true;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //Log4NetSetup(@"D:\Logs\MihaylovWeb\log.txt");
            var loggerPath = HostingEnvironment.MapPath("~/App_Data/Log/log.txt");            
            Log4NetSetup(loggerPath);

            var dataStoreFolder = HostingEnvironment.MapPath("~/App_Data/Google");
            kernel.Bind<IGoogleDriveApiHelper>().To<GoogleDriveApiHelper>().InSingletonScope()
                .WithConstructorArgument("dataStoreFolder", dataStoreFolder);


        }
    }
}
