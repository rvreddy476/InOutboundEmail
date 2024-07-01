using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Services.EmailTrailAPI.Models.DTO
{
    public class InboundEmailDto
    {
        public string EmailId { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
        public string TicketId { get; internal set; }

    }
}
