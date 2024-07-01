using System.ComponentModel.DataAnnotations;

namespace Email.Services.EmailTrailAPI.Models
{
    public class EmailTrail
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentDate { get; set; }
        public string TicketId { get; set; }
        public int? ParentEmailId { get; set; } // Nullable foreign key to represent replies
        public virtual EmailTrail ParentEmail { get; set; } // Navigation property for replies
        public virtual ICollection<EmailTrail> Replies { get; set; } // Navigation property for thread replies
        public DateTime Timestamp { get; internal set; }
        public string Sender { get; internal set; }
        public string Recipient { get; internal set; }
    }
}
