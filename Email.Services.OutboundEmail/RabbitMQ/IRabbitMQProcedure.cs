using Email.Services.OutboundEmail.Models;
using Email.Services.OutboundEmail.Models.DTO;

namespace Email.Services.OutboundEmail.RabbitMQ
{
    public interface IRabbitMQProcedure
    {
        void SendMessage(MailRequest request);
    }
}
