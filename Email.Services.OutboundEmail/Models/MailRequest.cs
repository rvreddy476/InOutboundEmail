using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NSwag.Annotations;

namespace Email.Services.OutboundEmail.Models
{
    public class MailRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UniqueId { get; set; }
        [Required(ErrorMessage = "Recipient email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        [Required]
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
       
        public string? AttachmentName { get; set; }
        public string? AttachmentURL { get; set; }
    }
}
