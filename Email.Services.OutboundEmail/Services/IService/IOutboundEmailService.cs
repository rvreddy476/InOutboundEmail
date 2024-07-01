using Email.Services.OutboundEmail.Models;

namespace Email.Services.OutboundEmail.Services.IService
{
    public interface IOutboundEmailService
    {

        Task<List<MailRequest>>? GetEmailTrailByTicketId(string ticketId);
        Task<MailRequest> OutboundIncomingEmail(MailRequest request, string path);
    }
}
