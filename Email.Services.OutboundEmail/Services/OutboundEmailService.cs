using Email.Services.OutboundEmail.Data;
using Email.Services.OutboundEmail.Models;
using Email.Services.OutboundEmail.Services.IService;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Options;
using MimeKit;

using System.Text.RegularExpressions;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Email.Services.OutboundEmail.Services
{
    public class OutboundEmailService : IOutboundEmailService
    {
        private readonly EmailConfig _mailSettings;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<OutboundEmailService> _logger;
        /// <summary>
        /// Initializes a new instance of the MailService class.
        /// </summary>
        public OutboundEmailService(EmailConfig mailSettings, AppDbContext dbContext, ILogger<OutboundEmailService> logger)
        {
            _mailSettings = mailSettings;
            _dbContext = dbContext;
            _logger = logger;
        }
        /// <summary>
        /// Save the details Send email with attachment Details.
        /// </summary>
        public async Task<MailRequest> OutboundIncomingEmail(MailRequest request, string attachmentFilePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.Subject))
                {
                    request.UniqueId = "Ticket:"+ Guid.NewGuid().ToString();
                   
                    // Save the email request to the database
                    request.Subject = request.Subject +  request.UniqueId;
                    request.UniqueId = GetUniqueIdentifierFromSubjectForOutbound(request.Subject);
                   
                    _dbContext.Add(request);
                    await _dbContext.SaveChangesAsync();

                    // Send the email with attachment
                    bool mailSent = await SendMailAsync(request, attachmentFilePath);

                    if (!mailSent)
                    {
                        Console.WriteLine("Failed to send outbound email or process attachment.");
                    }
                    _logger.LogInformation("Outbound email sent successfully.");
                }
                else
                {
                    Console.WriteLine("Content not linked to any outbound email. Sending auto-reply.");
                    _logger.LogWarning("Content not linked to any outbound email. Sending auto-reply.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while processing outbound email.");
                Console.WriteLine($"Error occurred: {ex.Message}");
                return request = null;
            }

            return request;
        }
        /// <summary>
        /// Send Email to Mail Id with mail subject and body.
        /// </summary>
        public async Task<bool> SendMailAsync(MailRequest mailRequest, string attachmentFilePath)
        {
            try
            {
                using (var emailMessage = new MimeMessage())
                {

                    emailMessage.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.UserName));

                    foreach (var address in mailRequest.ToEmail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        emailMessage.To.Add(new MailboxAddress(mailRequest.ToName, address.Trim()));
                    }

                    emailMessage.Subject = mailRequest.Subject;

                    var emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailRequest.Body;
                    if (!string.IsNullOrEmpty(attachmentFilePath) && System.IO.File.Exists(attachmentFilePath))
                    {
                        var attachment = new MimePart("application", "octet-stream")
                        {
                            Content = new MimeContent(System.IO.File.OpenRead(attachmentFilePath), ContentEncoding.Default),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = Path.GetFileName(attachmentFilePath)
                        };
                        emailBodyBuilder.Attachments.Add(attachment);
                    }

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (var mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.Auto);
                        await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Error occurred while sending email: {ex.Message}");
                _logger.LogError(ex, "Error occurred while sending email.");
                return false;
            }
        }
        /// <summary>
        /// Check Unique id is available at the time sending email
        /// </summary>
        public string GetUniqueIdentifierFromSubjectForOutbound(string subject)
        {
            try
            {
                //Regular expression pattern to match "Subject [Ticket:TicketId]" format
                Regex regex = new Regex(@"Ticket:(\d+)", RegexOptions.IgnoreCase);
                
                Match match = regex.Match(subject);
                if (match.Success && match.Groups.Count > 1)
                {
                    return match.Groups[1].Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ocuuerred at getting unique id", ex.Message);
            }
            return null;
        }
        /// <summary>
        /// Retrieves Outbond emails associated with a ticket ID.
        /// </summary>
        public async Task<List<MailRequest>>? GetEmailTrailByTicketId(string ticketId)
        {
            try
            {
                //Get Outbound Email List using TicketId
                var emailTrails = await _dbContext.MailRequests
                        .Where(et => et.UniqueId == ticketId)
                        .ToListAsync();
                return emailTrails ?? new List<MailRequest>();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occure to getting data using UniqueID :", ex.Message);
                return null;
            }
        }
    }
}









