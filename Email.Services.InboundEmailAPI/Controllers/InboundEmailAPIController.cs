using AutoMapper;
using Email.Services.InboundEmailAPI.Data;
using Email.Services.InboundEmailAPI.Models;
using Email.Services.InboundEmailAPI.Models.DTO;
using Email.Services.InboundEmailAPI.RabbitMQ;
using Email.Services.InboundEmailAPI.Services;
using Email.Services.InboundEmailAPI.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Web.Mvc;
using ControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Email.Services.InboundEmailAPI.Controllers
{
    [Route("api/inbound")]
    [ApiController]
    public class InboundEmailAPIController : ControllerBase
    {
        private readonly IInboundEmailService _inboundEmailServices;
        private IMapper _mapper;
        private readonly ILogger<InboundEmailAPIController> _logger;
        private readonly AppDbContext _dbContext;
        private ResponseDto _response;
       
        public InboundEmailAPIController(AppDbContext db, IInboundEmailService inboundEmailServices, IMapper mapper, ILogger<InboundEmailAPIController> logger)
        {
            _inboundEmailServices = inboundEmailServices;
            _mapper = mapper;
            _logger = logger;
            _dbContext = db;
            _response = new ResponseDto();            
        }
        /// <summary>
        /// Receives inbound emails.
        /// </summary>
        [HttpGet("receive")]
        public async Task<ActionResult<IEnumerable<MimeMessage>>> ReceiveEmails()
        {
            try
            {
                var emails = await _inboundEmailServices.ReceiveEmailsAsync();
                 
               // _response.Result = _mapper.Map<IEnumerable<InboundEmail>>(emails);
               // _rabbitMQ.ReveiveMessage(emails);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, new { message = "An error occurred while processing the request.", error = ex.Message });
            }
        }
        /// <summary>
        /// Retrieves inbound emails associated with a ticket ID.
        /// </summary>
        [HttpGet]
        [Route("GetInboundEmails/{ticketId}")]
        public async Task<List<InboundEmail>> GetEmailTrailByTicketIdAsync(string ticketId)
        {
            try
            {
                var emailTrails = _dbContext.InboundEmails.Where(e => Convert.ToBoolean(e.TicketId)).ToList();
                return emailTrails;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving email trails for ticket ID: {TicketId}", ticketId);
                Console.WriteLine(ex.Message);
                return new List<InboundEmail>();
            }
        }
    }
}
