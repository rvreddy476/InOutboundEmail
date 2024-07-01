using InboundOutbound.web.Models;
using InboundOutbound.web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InboundOutbound.web.Controllers
{
    public class InboundController : Controller
    {
        /// <summary>
        /// Declare a private readonly field for the inbound service
        /// </summary>
        private readonly IInboundService _inboundService;
        /// <summary>
        /// Constructor to initialize the inbound service through dependency injection
        /// </summary>
        /// <param name="inboundService"></param>
        public InboundController(IInboundService inboundService)
        {
            _inboundService = inboundService;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        ///  Asynchronous action method to get all inbound emails
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetAll()
        {
           List<InboundEmailDto>? list = new();

            //Call the service to get all inbound emails and store the response
            ResponseDto? response = await _inboundService.GetAllAsync();

            // Check if the response is not null and the call was successful
            if (response != null && response.IsSuccess)
            {
                // Deserialize the result into a list of InboundEmailDto
                list = JsonConvert.DeserializeObject<List<InboundEmailDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
