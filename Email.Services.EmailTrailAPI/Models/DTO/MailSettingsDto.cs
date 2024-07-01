namespace Email.Services.EmailTrailAPI.Models.DTO
{
    public class MailSettingsDto
    {
        public string UniqueId { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
