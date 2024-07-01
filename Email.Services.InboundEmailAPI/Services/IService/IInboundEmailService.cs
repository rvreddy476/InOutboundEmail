using Email.Services.InboundEmailAPI.Models;
using MimeKit;

namespace Email.Services.InboundEmailAPI.Services.IService
{
    public interface IInboundEmailService
    {
        public Task<InboundEmail> InboundIncomingEmail(InboundEmail request);
        string GetMessageContent(MimeMessage message);
        Task<List<InboundEmail>> ReceiveEmailsAsync();
    }
}
