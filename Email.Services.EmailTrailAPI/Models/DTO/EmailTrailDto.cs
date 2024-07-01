namespace Email.Services.EmailTrailAPI.Models.DTO
{
    public class EmailTrailDto
    {
        public string TicketId { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
