[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ExamService.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ExamService.App_Start.NinjectWebCommon), "Stop")]

namespace ExamService.App_Start
{
    using System;
    using System.Web;
    using System.Web.Http;
    
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.WebApi;
    using Ninject.Web.Common;

    using ExamService.Repositories;
    using ExamService.Services;

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
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();


                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
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
            kernel.Bind<IGradeRepository>().To<GradeRepository>();
            kernel.Bind<IExamRepository>().To<ExamRepository>();
            kernel.Bind<ISubjectRepository>().To<SubjectRepository>();
            kernel.Bind<IGradeService>().To<GradeService>();
            kernel.Bind<IIcasExamService>().To<IcasExamService>();
            kernel.Bind<ISubjectService>().To<SubjectService>();
        }        
    }
}
