namespace InboundOutbound.web.Models
{
    public class EmailTrailDto
    {
        public string TicketId { get; set; }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
    }
}
