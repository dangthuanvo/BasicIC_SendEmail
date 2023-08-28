using BasicIC_SendEmail.Models.Kafka;
using BasicIC_SendEmail.Models.Main;
using Common.Commons;
using System.Threading.Tasks;

namespace BasicIC_SendEmail.Services.Interfaces
{
    public interface ISendEmailService
    {
        Task<ResponseService<bool>> SendEmailOrderConfirm(KafkaEmailModel email);
        Task<ResponseService<bool>> SendEmailAccountConfirm(EmailModel email);
    }
}