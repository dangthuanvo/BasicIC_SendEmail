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
using System.Threading.Tasks;

namespace BasicIC_SendEmail.Services.Implement
{
    public class SendEmailService : BaseService, ISendEmailService
    {
        public SendEmailService(ILogger logger, IConfigManager config, IMapper mapper) : base(config, logger, mapper)
        {

        }
        public async Task<ResponseService<bool>> SendEmailAsync(KafkaEmailModel email)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "dangthuanvo1611@gmail.com";
                string smtpPassword = "pwuebwfhtmnujdwt";

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.UseDefaultCredentials = false;

                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;

                    var message = new MailMessage();
                    message.From = new MailAddress(smtpUsername);
                    message.To.Add(email.toEmail);
                    message.Subject = email.subject;
                    message.Body = email.body;
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