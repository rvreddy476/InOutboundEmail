using Email.Services.InboundEmailAPI.Models;
using Email.Services.InboundEmailAPI.Models.DTO;

namespace Email.Services.InboundEmailAPI.RabbitMQ
{
    public interface IRabbitMQProcedure
    {
        void ReveiveMessage(List<InboundEmail> mail);
    }
}
