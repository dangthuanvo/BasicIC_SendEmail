
using BasicIC_SendEmail.App_Start;
using BasicIC_SendEmail.Common;
using Common.Commons;
using Common.Interfaces;
using Common.Params.Base;
using Settings.Common;
using System.Threading.Tasks;
using System.Web.Http;


namespace BasicIC_SendEmail.ApiControllers.Controllers
{
    [RoutePrefix("api/base")]
    public class BaseController : ApiController
    {
        public BaseController() : base() { }

    }
    [RoutePrefix("api/consult")]
    public class ConsultController : ApiController
    {
        private static ILogger _logger = NinjectWebCommon.CreateInstanceDJ<ILogger>();

        public ConsultController() : base() { }

        [Route("health-check")]
        [HttpGet]
        public IHttpActionResult HealthCheck()
        {
            _logger.LogInfo(CommonFunc.GetMethodName(new System.Diagnostics.StackTrace()));
            return Ok();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register()
        {
            ResponseService<bool> response = await ConsultClient.RegisterService();
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return new ResponseFail<bool>().Error(response);
            }
        }

        [Route("unregister")]
        [HttpPost]
        public async Task<IHttpActionResult> UnRegister()
        {
            ResponseService<bool> response = await ConsultClient.UnRegisterService();
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return new ResponseFail<bool>().Error(response);
            }
        }

        //[Authorized]
        [Route("send-topic")]
        [HttpPost]
        public async Task<IHttpActionResult> SendTopic(TopicParam param)
        {
            ResponseService<bool> response = await ConsultClient.SendTopic(param);
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return new ResponseFail<bool>().Error(response);
            }
        }

        //[Authorized]
        [Route("subscribe-topic")]
        [HttpPost]
        public IHttpActionResult SubscribeTopic(TopicSubscribe param)
        {
            ResponseService<bool> response = ConsultClient.SubscribeTopic(param);
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return new ResponseFail<bool>().Error(response);
            }
        }

        //[Authorized]
        [Route("unsubscribe-topic")]
        [HttpPost]
        public IHttpActionResult UnSubscribeTopic(TopicSubscribe param)
        {
            ResponseService<bool> response = ConsultClient.UnSubscribeTopic(param);
            if (response.status)
            {
                return Ok(response);
            }
            else
            {
                return new ResponseFail<bool>().Error(response);
            }
        }
    }

}