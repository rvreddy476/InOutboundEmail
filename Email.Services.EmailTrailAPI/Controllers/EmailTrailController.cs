using AutoMapper;
using Email.Services.EmailTrailAPI.Models.DTO;
using Email.Services.EmailTrailAPI.Models;
using Email.Services.EmailTrailAPI.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Email.Services.EmailTrailAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Email.Services.EmailTrailAPI.Controllers
{
    [Route("api/EmailTrail")]
    [ApiController]
    public class EmailTrailController : ControllerBase
    {
        private readonly ILogger<EmailTrailController> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailTrailService _emailTrailService;
        private ResponseDto _response;
        private readonly AppDbContext _dbContext;
        public EmailTrailController(ILogger<EmailTrailController> logger, IMapper mapper, IEmailTrailService emailTrailService)
        {
            _logger = logger;
            _mapper = mapper;
            _emailTrailService = emailTrailService;
            _response = new ResponseDto();           
        }

        /// <summary>
        /// Retrieves email trails associated with a ticket ID.
        /// </summary>
        [HttpGet]
        [Route("GetEmailTrailsByTicketId/{ticketId}")]
        public async Task<ActionResult<List<EmailTrail>>> GetEmailTrailsByTicketId(string ticketId)
        {
            try
            {
                // Retrieve outbound and inbound email trails
                IEnumerable<MailSettingsDto> outBoundDtos = await _emailTrailService.GetOutBoundEmails(ticketId);
                IEnumerable<InboundEmailDto> inboundEmailDtos = await _emailTrailService.GetInBoundEmails(ticketId);

                // Merge outbound and inbound email trails into a single list
                var mergedEmails = outBoundDtos.Select(dto => _mapper.Map<MergedEmailDto>(dto))
                    .Concat(inboundEmailDtos.Select(dto => _mapper.Map<MergedEmailDto>(dto))).ToList();

                if (mergedEmails == null || mergedEmails.Count() == 0)
                {
                    // No email trails found for the given ticket ID
                    return NotFound("No email trails found for the given ticket ID: " + ticketId);
                }
                _response.Result = mergedEmails;

                return Ok(_response);
              
            }
            catch (Exception ex)
            {
                // Log and return an error message if an exception occurs
                _logger.LogError(ex, "Error occurred while retrieving email trails for ticket ID: {TicketId}", ticketId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request");
            }
        }
    }
}
