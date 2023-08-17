using AutoMapper;
using BasicIC_SendEmail.Config;
using BasicIC_SendEmail.KafkaComponents;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BasicIC_SendEmail
{
    public class MvcApplication : System.Web.HttpApplication
    {
        internal static MapperConfiguration MapperConfiguration { get; private set; }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MapperConfiguration = AutoMapperConfig.MapperConfiguration();
            log4net.Config.XmlConfigurator.Configure();
            HostingEnvironment.QueueBackgroundWorkItem(async ct =>
            {
                ConsumerWrapper _consumer = new ConsumerWrapper();
                await _consumer.StartGetMess(new string[]
                {
                    Topic.SEND_EMAIL
                });
            });
        }
    }
}
