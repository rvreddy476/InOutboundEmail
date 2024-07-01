
using InboundOutbound.web.Models;
using InboundOutbound.web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InboundOutbound.web.Controllers
{
    public class OutboundController : Controller
    {
        private readonly IOutboundService _outboundService;
        public OutboundController(IOutboundService outboundService)
        {
            _outboundService = outboundService;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Display all Outgoing Email
        /// </summary>
        /// <returns>it display all outgoing email</returns>
        public async Task<IActionResult> GetAll()
        {
            List<MailRequest>? list = new();

            // Call the service to get all outbound emails and store the response
            ResponseDto? response = await _outboundService.GetAllAsync();
            // Check if the response is not null and the call was successful
            if (response != null && response.IsSuccess)
            {
                // Deserialize the result into a list of MailRequest
                list = JsonConvert.DeserializeObject<List<MailRequest>>(Convert.ToString(response.Result));
            }
            // Return the view with the list of outbound emails
            return View(list);
        }

        /// <summary>
        ///   Action method to render the send mail view
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SendMail()
        {
            return View();
        }

        /// <summary>
        /// Action method to handle POST request for sending mail
        /// </summary>
        /// <param name="mailRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMail(MailRequestDto mailRequest)
        {
            if(ModelState.IsValid)
            {
                // Call the service to send the email and store the response
                ResponseDto? response = await _outboundService.SendEmailAsync(mailRequest);

                // Check if the response is not null and the call was successful
                if (response != null && response.IsSuccess)
                {
                    // Set success message in TempData and redirect to GetAll action
                    TempData["Success"] = "Mail sent Successfully";
                    return RedirectToAction("GetAll");
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            // Return the view with the mail request data
            return View(mailRequest);
        }
    }
}
