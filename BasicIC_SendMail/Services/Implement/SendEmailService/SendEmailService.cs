using AutoMapper;
using BasicIC_SendEmail.Interfaces;
using BasicIC_SendEmail.Models.Kafka;
using BasicIC_SendEmail.Services.Interfaces;
using Common;
using Common.Commons;
using Common.Interfaces;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BasicIC_SendEmail.Services.Implement
{
    public class SendEmailService : BaseService, ISendEmailService
    {
        public SendEmailService(ILogger logger, IConfigManager config, IMapper mapper) : base(config, logger, mapper)
        {

        }
        public async Task<ResponseService<bool>> SendEmailAsync(KafkaEmailModel emailContent)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = Constants.EMAIL_ADDRESS_HOST;
                string smtpPassword = Constants.EMAIL_ADDRESS_PASS;
                //string body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("Service/Implement/SendEmailService/EmailTemplate.html"));
                string filePath = HttpContext.Current.Server.MapPath("~/Service/Implement/SendEmailService/EmailTemplate.html");


                //Debug.WriteLine($"Reading file from path: {filePath}");
                string body = System.IO.File.ReadAllText(filePath, Encoding.UTF8);

                body = body.Replace("{{first_name}}", emailContent.customer_name);
                body = body.Replace("{{order_id}}", emailContent.order_id);
                string listProduct = null;
                foreach (var productData in emailContent.orderDetailModel)
                {
                    //string product = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("Service/Implement/SendEmailService/ProductTemplate.html"));
                    body = body.Replace("{{order.items.title}}", productData.product_name);
                    listProduct += System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("Service/Implement/SendEmailService/EmailTemplate.html")).Replace("{{order.items.title}}", productData.product_name).Replace("{{order.items.quantity}}", productData.quantity.ToString()).Replace("{{order.items.price}}", productData.product_price.ToString());
                }
                body = body.Replace("<!--ADD PRODUCT HERE-->", listProduct);
                //string product = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("SendEmailService/ProductTemplate.html"));

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