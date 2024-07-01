using NSwag.Annotations;

namespace Email.Services.InboundEmailAPI.Models.DTO
{
    public class MailRequestDto
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
