using Email.Services.EmailTrailAPI.Data;
using Email.Services.EmailTrailAPI.Models;
using Email.Services.EmailTrailAPI.Models.DTO;
using Email.Services.EmailTrailAPI.Services.IService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Email.Services.EmailTrailAPI.Services
{
    public class EmailTrailService : IEmailTrailService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _context;
        private readonly Dictionary<string, List<EmailTrail>> _emailData = new Dictionary<string, List<EmailTrail>>();
        /// <summary>
        /// Initializes a new instance of the EmailTrailService class.
        /// </summary>
        public EmailTrailService(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// Retrieves email trails associated with a ticket ID asynchronously.
        /// </summary>
        public async Task<List<EmailTrail>> GetEmailTrailByTicketIdAsync(string ticketId)
        {
            if (_emailData.ContainsKey(ticketId))
            {
                var emailTrails = _emailData[ticketId];
                var sortedEmailTrails = emailTrails.OrderBy(e => e.Timestamp).ToList();
                return sortedEmailTrails;
            }
            return await Task.FromResult(new List<EmailTrail>());
        }
        /// <summary>
        /// Retrieves all email trails from the database.
        /// </summary>
        public List<EmailTrail> GetAllEmailTrails()
        {
            return _context.EmailTrails.ToList();
        }
        /// <summary>
        /// Retrieves inbound emails associated with a ticket ID.
        /// </summary>
        public async Task<IEnumerable<InboundEmailDto>> GetInBoundEmails(string TicketId)
        {
            var client = _httpClientFactory.CreateClient("receive");
            var response = await client.GetAsync($"/api/inbound/GetInboundEmails/{TicketId}");
            var apiContet = await response.Content.ReadAsStringAsync();
            // Deserialize JSON response to list of InboundEmailDto
            var mailRequests = JsonConvert.DeserializeObject<List<InboundEmailDto>>(apiContet);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error response
                response.EnsureSuccessStatusCode();
                // Thrown an exception or return an empty list
                return new List<InboundEmailDto>();
            }

            // Convert MailRequest to InboundEmailDto
            var mailSettingsList = mailRequests.Select(mailRequest => new InboundEmailDto
            {
                TicketId = TicketId,
                From = mailRequest.From,
                EmailId = mailRequest.EmailId,
                Subject = mailRequest.Subject,
                Body = mailRequest.Body,
                Attachments = mailRequest.Attachments
            });
            
            return mailSettingsList;
        }
        public async Task<IEnumerable<MailSettingsDto>> GetOutBoundEmails(string TicketId)
        {
            var client = _httpClientFactory.CreateClient("SendMail");
            var response = await client.GetAsync($"/api/Outbound/GetEmails/{TicketId}");
            var apiContet = await response.Content.ReadAsStringAsync();
            // Deserialize JSON response to list of MailSettingsDto
            var mailRequests = JsonConvert.DeserializeObject<List<MailSettingsDto>>(apiContet);
            if (!response.IsSuccessStatusCode)
            {
                // Handle error response
                response.EnsureSuccessStatusCode();
                return new List<MailSettingsDto>();
            }
            // Convert MailRequest to MailSettingsDto
            var mailSettingsList = mailRequests.Select(mailRequest => new MailSettingsDto
            {
                UniqueId = mailRequest.UniqueId,
                ToEmail = mailRequest.ToEmail,
                ToName = mailRequest.ToName,
                Subject = mailRequest.Subject,
                Body = mailRequest.Body,
                Attachments = mailRequest.Attachments
            });
            
            return mailSettingsList;
        }
    }
}
