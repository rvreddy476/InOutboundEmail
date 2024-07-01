namespace InboundOutbound.web.Models
{
    public class MailRequestDto
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IFormFile? Attachments { get; set; }
        
    }
}
