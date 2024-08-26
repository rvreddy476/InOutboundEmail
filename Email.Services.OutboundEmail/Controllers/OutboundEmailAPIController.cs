using AutoMapper;
using Email.Services.OutboundEmail.Models.DTO;
using Email.Services.OutboundEmail.Models;
using MailKit;
using Microsoft.AspNetCore.Mvc;
using Email.Services.OutboundEmail.Services.IService;
using Email.Services.OutboundEmail.Data;
using Email.Services.OutboundEmail.RabbitMQ;

namespace Email.Services.OutboundEmail.Controllers
{
    /// <summary>
    /// Controller responsible for handling outbound email services.
    /// </summary>
    [ApiController]
    [Route("api/Outbound")]
    public class OutboundEmailAPIController : ControllerBase
    {
        private readonly IOutboundEmailService _mailService;
        private readonly ILogger<OutboundEmailAPIController> _logger;
        private IMapper _mapper;
        private ResponseDto _response;
        private readonly AppDbContext _dbContext;
        private readonly IRabbitMQProcedure _rabbitMQ;
        /// <summary>
        /// Initializes a new instance of the OutboundEmailServicesController class.
        /// </summary>
        public OutboundEmailAPIController(IOutboundEmailService mailService,AppDbContext dbContext, ILogger<OutboundEmailAPIController> logger, IMapper mapper)
        {
            _mailService = mailService;
            _logger = logger;
            _mapper = mapper;
            _response = new ResponseDto();
            _dbContext = dbContext;
            
        }
        [HttpGet]
        public object Get()
        {
            try
            {
                IEnumerable<MailRequest> objlist = _dbContext.MailRequests.ToList();
                _response.Result = _mapper.Map<IEnumerable<MailRequest>>(objlist);

            }
            catch
            {

            }
            return _response;
        }
        /// <summary>
        /// Sends an email.
        /// </summary>
        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail([FromForm] MailRequestDto mailData)
        {
            try
            {
                MailRequest emailData = _mapper.Map<MailRequest>(mailData);
               
                if (mailData == null)
                {
                    _logger.LogError("Mail data cannot be null");
                    return BadRequest("Mail data cannot be null");
                }
                var attachments = mailData.Attachments;
                if (attachments == null)
                {
                    await _mailService.OutboundIncomingEmail(emailData, null);
                }
                else
                {
                    foreach (var attachment in attachments)
                    {
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");
                        Directory.CreateDirectory(folderPath);

                        // Save the file to the folder
                        var fileName = Path.GetFileName(attachment.FileName);
                        var filePath = Path.Combine(folderPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await attachment.CopyToAsync(stream);
                        }
                        
                        emailData.AttachmentURL = filePath;
                        emailData.AttachmentName = fileName;
                        
                        // Send email with attachment
                        var emailSend = await _mailService.OutboundIncomingEmail(emailData, filePath);
                    }
                }
               
               // _response.Result = _mapper.Map<MailRequestDto>(emailData);
               
               // _rabbitMQ.SendMessage(emailData);

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending mail");
                return StatusCode(500, "An error occurred while sending the mail");
            }
        }
        /// <summary>
        /// Retrieves email trails by ticket ID.
        /// </summary>
        [HttpGet]
        [Route("GetEmails/{ticketId}")]
        public async Task<List<MailRequest>> GetEmailTrailByTicketIdAsync(string ticketId)
        {
            try
            {
                var emailTrails = await _mailService.GetEmailTrailByTicketId(ticketId);
                return emailTrails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving email trails for ticket ID: {TicketId}", ticketId);
                throw new Exception("An error occurred while retrieving email trails.");
            }
        }
    }
}





























//using AutoMapper;
//using Email.Services.OutboundEmail.Models;
//using Email.Services.OutboundEmail.Models.DTO;
//using Email.Services.OutboundEmail.Services.IService;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Org.BouncyCastle.Asn1.Pkcs;


//namespace Email.Services.OutboundEmail.Controllers
//{
//    [Route("api/outbound")]
//    [ApiController]
//    public class OutboundEmailController : ControllerBase
//    {
//        private readonly IOutboundEmailService _mailService;
//        private readonly ILogger<OutboundEmailController> _logger;
//        private IMapper _mapper;
//        public OutboundEmailController(IOutboundEmailService mailService, ILogger<OutboundEmailController> logger, IMapper mapper)
//        {
//            _mailService = mailService;
//            _logger = logger;
//            _mapper = mapper;
//        }

//        [HttpPost("send")]
//        public async Task<IActionResult> SendEmail([FromBody] MailRequestDto requestDto)
//        {

//            try
//            {
//                MailRequest request = _mapper.Map<MailRequest>(requestDto);
//                if (requestDto == null)
//                {
//                    _logger.LogError("Mail data cannot be null");
//                    return BadRequest("Mail data cannot be null");
//                }
//                var attachments = requestDto.Attachments;
//                if (attachments == null)
//                {
//                    await _mailService.SendEmailAsync(request, null);
//                }
//                else
//                {
//                    foreach (var attachment in attachments)
//                    {
//                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");
//                        Directory.CreateDirectory(folderPath);

//                        // Save the file to the folder
//                        var fileName = Path.GetFileName(attachment.FileName);
//                        var filePath = Path.Combine(folderPath, fileName);
//                        using (var stream = new FileStream(filePath, FileMode.Create))
//                        {
//                            await attachment.CopyToAsync(stream);
//                        }
//                        request.AttachmentURL = filePath;
//                        request.AttachmentName = fileName;
//                        // Send email with attachment
//                        var emailSend = await _mailService.SendEmailAsync(request, filePath);
//                    }
//                }
//                if (requestDto.UniqueId == null)
//                {
//                    return BadRequest("Subject not contain Ticket ID or in  [Ticket:000] this format");
//                }
//               // await _mailService.SendEmailAsync(request);
//                return Ok("Mail sent successfully");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error sending mail");
//                return StatusCode(500, "An error occurred while sending the mail");
//            }
//            //try
//            //{
//            //    await _mailService.SendEmailAsync(request);
//            //    return Ok("Email sent successfully.");
//            //}
//            //catch (Exception ex)
//            //{
//            //    return StatusCode(500, $"An error occurred: {ex.Message}");
//            //}
//        }
//    }
//}
