using BasicIC_SendEmail.Models.Kafka;
using Common.Commons;
using System.Threading.Tasks;

namespace BasicIC_SendEmail.Services.Interfaces
{
    public interface ISendEmailService
    {
        Task<ResponseService<bool>> SendEmailAsync(KafkaEmailModel email);
    }
}