using BasicIC_SendEmail.CustomAttributes;
using BasicIC_SendEmail.Models.Kafka;
using BasicIC_SendEmail.Models.Main;
using BasicIC_SendEmail.Services.Interfaces;
using Common.Commons;
using Settings.Common;
using System.Threading.Tasks;
using System.Web.Http;

namespace BasicIC_SendEmail.ApiControllers.Controllers
{
    //[Authorized]
    [RoutePrefix("api/email")]
    public class SendEmailController : ApiController
    {
        private readonly ISendEmailService _sendEmailService;

        public SendEmailController(ISendEmailService sendEmailService)
        {
            _sendEmailService = sendEmailService;
        }

        [Route("send-email-order-confirm")]
        [ValidateModel]
        [HttpPost]
        public async Task<IHttpActionResult> SendEmailOrderConfirm(KafkaEmailModel param)
        {
            ResponseService<bool> response = await _sendEmailService.SendEmailOrderConfirm(param);
            if (response.status)
                return Ok(response);

            return new ResponseFail<bool>().Error(response);
        }

        [Route("send-email-account-confirm")]
        [ValidateModel]
        [HttpPost]
        public async Task<IHttpActionResult> SendEmailAccountConfirm(EmailModel param)
        {
            ResponseService<bool> response = await _sendEmailService.SendEmailAccountConfirm(param);
            if (response.status)
                return Ok(response);

            return new ResponseFail<bool>().Error(response);
        }

    }
}
