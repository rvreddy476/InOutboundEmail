using AutoMapper;
using Email.Services.InboundEmailAPI.Data;
using Email.Services.InboundEmailAPI.Migrations;
using Email.Services.InboundEmailAPI.Models;
using Email.Services.InboundEmailAPI.Models.DTO;
using Email.Services.InboundEmailAPI.Services.IService;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.RegularExpressions;

namespace Email.Services.InboundEmailAPI.Services
{
    public class InboundEmailService : IInboundEmailService
    {
        private readonly IMapper _mapper;
        private readonly ResponseDto _response;
        private readonly AppDbContext _dbContext;
        private readonly EmailConfig _mailSettings;
        private readonly List<EmailDto> _outboundEmailDto;

        /// <summary>
        /// Initializes a new instance of the InboundEmailServices class.
        /// </summary>
        public InboundEmailService(AppDbContext db,
            IMapper mapper, EmailConfig mailSettings, List<EmailDto> outboundEmailDto)
        {
            _mapper = mapper;
            _dbContext = db;
            _mailSettings = mailSettings;
            _outboundEmailDto = outboundEmailDto; // Assign the injected List<OutBoundEmailDto>
        }
        /// <summary>
        /// Processes an incoming inbound email.
        /// </summary>
        public async Task<InboundEmail> InboundIncomingEmail(InboundEmail request)
        {
            if (request.Subject.Contains(request.Subject))
            {
                await ReceiveEmailsAsync();
                _dbContext.Add(request);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Content not linked to any outbound email. Sending auto-reply.");
                SendAutoReply(request);
            }
            return request;
        }
        /// <summary>
        /// Gets the message content from a MimeMessage.
        /// </summary>
        public string GetMessageContent(MimeMessage message)
        {
            if (message.HtmlBody != null)
            {
                return message.HtmlBody;
            }
            else if (message.TextBody != null)
            {
                return message.TextBody;
            }
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Receives inbound emails asynchronously.
        /// </summary>
        public async Task<List<InboundEmail>> ReceiveEmailsAsync()
        {
            var inboundEmails = new List<InboundEmail>();
            try
            {
                using (var imapClient = new ImapClient())
                {
                    // Connect to the IMAP server
                    imapClient.Connect(_mailSettings.Host, _mailSettings.Port, true);
                    await imapClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);

                    // Open the INBOX folder
                    var mailFolder = await imapClient.GetFolderAsync("INBOX");
                    await mailFolder.OpenAsync(FolderAccess.ReadOnly);

                    // Fetch the latest 20 messages
                    var messageSummaries = await mailFolder.FetchAsync(mailFolder.Count - 20, -1, MessageSummaryItems.UniqueId);

                    foreach (var summary in messageSummaries)
                    {
                        var message = await mailFolder.GetMessageAsync(summary.UniqueId);

                        // Create a new InboundEmail object
                        var emailContent = new InboundEmail
                        {

                            Subject = message.Subject,
                            EmailId = message.From.ToString(),
                            From = message.From.ToString(),
                            Date = message.Date.DateTime,
                            Body = GetMessageBody(message.Body),
                            EmailAttachments = GetMessageAttachments(message.Attachments),
                            // Extract the ticket ID from the subject
                            TicketId = ExtractTicketId(message.Subject)

                        };
                       
                        var outboundEmail = SearchForOutboundEmail(emailContent.TicketId);
                       
                        inboundEmails.Add(emailContent);

                        _dbContext.InboundEmails.Add(emailContent);
                        await _dbContext.SaveChangesAsync();
                    }

                    await mailFolder.CloseAsync(false);
                    await imapClient.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                // You might also want to throw the exception or handle it differently based on your application's requirements
            }

            return inboundEmails;
        }
        /// <summary>
        /// Extracts the ticket ID from the subject.
        /// </summary>
        private string ExtractTicketId(string subject)
        {
            // Regular expression pattern to match "Ticket #123: Issue" format
            string pattern = @"Ticket:(\d+)";

            // Match the pattern in the subject string
            Match match = Regex.Match(subject, pattern);

            // If a match is found, extract the ticket ID
            if (match.Success)
            {
                return match.Groups[1].Value; // Extracted ticket ID
            }
            else
            {
                return null; // No ticket ID found
            }
        }
        /// <summary>
        /// Searches for an outbound email with the specified unique identifier.
        /// </summary>
        private EmailDto SearchForOutboundEmail(string uniqueIdentifier)
        {
            // Example logic: Search for the corresponding outbound email message in a collection
            return _outboundEmailDto.FirstOrDefault(email => email.UniqueIdentifier == uniqueIdentifier);
        }
        /// <summary>
        /// Gets the message body from a MimeEntity.
        /// </summary>
        private string GetMessageBody(MimeEntity body)
        {
            if (body is TextPart textPart)
            {
                return textPart.Text;
            }
            else if (body is Multipart multipart)
            {
                foreach (var part in multipart)
                {
                    if (part is TextPart text)
                    {
                        return text.Text;
                    }
                    else if (part is TextPart html)
                    {
                        return html.Text;
                    }
                    else if (part is Multipart nestedMultipart)
                    {
                        return GetMessageBody(nestedMultipart);
                    }
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Gets the message attachment from Email.
        /// </summary>
        private List<EmailAttachments> GetMessageAttachments(IEnumerable<MimeEntity> attachments)
        {
            var attachmentList = new List<EmailAttachments>();

            foreach (var attachment in attachments.OfType<MimePart>())
            {
                var attachmentStream = new MemoryStream();
                attachment.Content.DecodeTo(attachmentStream);

                // Ensure that the stream is positioned at the beginning
                attachmentStream.Position = 0;

                attachmentList.Add(new EmailAttachments(attachment.FileName, Convert.ToBase64String(attachmentStream.ToArray())));
            }

            return attachmentList;
        }
        public void SendAutoReply(InboundEmail request)
        {
            try
            {
                // Create and send auto-reply message
                // Code remains the same as in the original SendAutoReply method
               // Console.WriteLine($"Sending auto-reply to {request.From}...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending auto-reply: {ex.Message}");
            }
        }

       
    }
}
