using System.ComponentModel.DataAnnotations;

namespace Email.Services.InboundEmailAPI.Models
{
    public class EmailAttachments
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public EmailAttachments(string fileName, string content)
        {
            FileName = fileName;
            Content = content;
        }
    }
}
