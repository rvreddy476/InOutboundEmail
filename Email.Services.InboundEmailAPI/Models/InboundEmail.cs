using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Email.Services.InboundEmailAPI.Models
{
    public class InboundEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string EmailId { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Recipient email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string From { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        [NotMapped]
        public List<EmailAttachments>? EmailAttachments { get; set; }
        public string? TicketId { get; set; }
    }
}
