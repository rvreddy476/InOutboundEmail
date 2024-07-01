using Email.Services.EmailTrailAPI.Models.DTO;
using Email.Services.EmailTrailAPI.Models;

namespace Email.Services.EmailTrailAPI.Services.IService
{
    public interface IEmailTrailService
    {
        List<EmailTrail> GetAllEmailTrails();
        Task<List<EmailTrail>> GetEmailTrailByTicketIdAsync(string ticketId);
        Task<IEnumerable<InboundEmailDto>> GetInBoundEmails(string TicketId);
        Task<IEnumerable<MailSettingsDto>> GetOutBoundEmails(string TicketId);
    }
}
