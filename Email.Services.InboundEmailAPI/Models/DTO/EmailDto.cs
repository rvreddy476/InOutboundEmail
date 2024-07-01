namespace Email.Services.InboundEmailAPI.Models.DTO
{
    public class EmailDto
    {
        public int TemplateID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string UniqueIdentifier { get; internal set; }
    }
}
