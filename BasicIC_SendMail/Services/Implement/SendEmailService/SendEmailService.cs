using AutoMapper;
using BasicIC_SendEmail.Common;
using BasicIC_SendEmail.Interfaces;
using BasicIC_SendEmail.Models.Kafka;
using BasicIC_SendEmail.Models.Main;
using BasicIC_SendEmail.Services.Interfaces;
using Common;
using Common.Commons;
using Common.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BasicIC_SendEmail.Services.Implement
{
    public class SendEmailService : BaseService, ISendEmailService
    {
        public SendEmailService(ILogger logger, IConfigManager config, IMapper mapper) : base(config, logger, mapper)
        {

        }

        public async Task<ResponseService<bool>> SendEmailAccountConfirm(EmailModel param)
        {
            try
            {
                string smtpServer = CommonFunc.GetValueDefaultCommonSettingFromMemory("smtpServer");
                int smtpPort = 587;
                string smtpUsername = Constants.EMAIL_ADDRESS_HOST;
                string smtpPassword = Constants.EMAIL_ADDRESS_PASS;
                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;
                    var message = new MailMessage();
                    message.From = new MailAddress(smtpUsername);
                    message.To.Add(param.toEmail);
                    message.Subject = param.toEmail;
                    message.Body = param.toEmail;
                    message.IsBodyHtml = true;
                    await client.SendMailAsync(message);
                }

                return new ResponseService<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResponseService<bool>(ex.Message).BadRequest(ErrorCodes.UNHANDLED_ERROR);
            }

        }

        public async Task<ResponseService<bool>> SendEmailOrderConfirm(KafkaEmailModel emailContent)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = Constants.EMAIL_ADDRESS_HOST;
                string smtpPassword = Constants.EMAIL_ADDRESS_PASS;
                string body = System.IO.File.ReadAllText(AppContext.BaseDirectory + "Services\\Implement\\SendEmailService\\EmailTemplate.html");
                string listProduct = null;
                string first_name = emailContent.customer_name;
                string order_id = emailContent.order_id;
                string order_shipping_address = emailContent.shipping_address;
                string order_shipping_price = emailContent.shipping_fee.ToString();
                string order_total_price = emailContent.total_price.ToString();
                string company_name = Constants.COMPANY_NAME;
                string company_address = Constants.COMPANY_ADDRESS;
                foreach (var productData in emailContent.orderDetailModel)
                {
                    listProduct += System.IO.File.ReadAllText(AppContext.BaseDirectory + "Services\\Implement\\SendEmailService\\ProductTemplate.html")
                    .Replace("{{order.items.title}}", productData.product_name)
                    .Replace("{{order.items.quantity}}", productData.quantity.ToString())
                    .Replace("{{order.items.price}}", productData.product_price.ToString());
                }
                body = body.Replace("{{first_name}}", first_name)
                .Replace("{{order_id}}", order_id)
                .Replace("{{order_shipping_address}}", order_shipping_address)
                .Replace("{{order_shipping_price}}", order_shipping_price)
                .Replace("{{order_total_price}}", order_total_price)
                .Replace("{{company_name}}", company_name)
                .Replace("{{company_address}}", company_address)
                .Replace("<!--ADD PRODUCT HERE-->", listProduct);

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;
                    var message = new MailMessage();
                    message.From = new MailAddress(smtpUsername);
                    message.To.Add(emailContent.to_email);
                    message.Subject = emailContent.subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    await client.SendMailAsync(message);
                }

                return new ResponseService<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResponseService<bool>(ex.Message).BadRequest(ErrorCodes.UNHANDLED_ERROR);
            }
        }
    }
}