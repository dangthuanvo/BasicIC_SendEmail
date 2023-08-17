[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(BasicIC_SendEmail.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(BasicIC_SendEmail.App_Start.NinjectWebCommon), "Stop")]

namespace BasicIC_SendEmail.App_Start
{
    using AutoMapper;
    using BasicIC_SendEmail.Config;
    using BasicIC_SendEmail.Interfaces;
    using BasicIC_SendEmail.Services.Implement;
    using BasicIC_SendEmail.Services.Interfaces;
    using global::Common.Commons;
    using global::Common.Interfaces;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Repository.Interfaces;
    using Repository.Repositories;
    using System;
    using System.Web;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        private static IKernel kernel;

        /// <summary>
        /// Starts the application.
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
            kernel = new StandardKernel();
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

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Inject common
            kernel.Bind<ILogger>().To<Logger>();

            kernel.Bind<IConfigManager>().To<ConfigManager>();

            kernel.Bind<IMapper>().ToMethod(context =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                    cfg.ConstructServicesUsing(t => kernel.Get(t));
                });
                return config.CreateMapper();
            }).InSingletonScope(); ;
            kernel.Bind(typeof(IRepositorySql<>)).To(typeof(BaseRepositorySql<>));
            kernel.Bind(typeof(IRepositorySql<>)).To(typeof(BasicIC_SendEmailRepository<>));
            kernel.Bind<ISendEmailService>().To<SendEmailService>();
        }
        public static T CreateInstanceDJ<T>()
        {
            return kernel.Get<T>();
        }
    }
}