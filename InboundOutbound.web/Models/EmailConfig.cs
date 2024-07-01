using System.ComponentModel.DataAnnotations;

namespace InboundOutbound.web.Models
{
    public class EmailConfig
    {
        [Key]
        public int Id { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
