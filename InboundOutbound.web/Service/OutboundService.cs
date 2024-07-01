using InboundOutbound.web.Models;
using InboundOutbound.web.Service.IService;
using InboundOutbound.web.Utility;

namespace InboundOutbound.web.Service
{
    public class OutboundService : IOutboundService
    {
        private readonly IBaseService _baseService;
        public OutboundService(IBaseService baseService)
        {
           _baseService = baseService;
        }

        /// <summary>
        /// Method to retrieve all outbound emails
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto?> GetAllAsync()
        {
            // Call the base service to send a GET request to retrieve all outbound emails
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Sd.ApiType.GET,                      
                Url = Sd.OutboundAPIBase + "/api/Outbound/"
            });
        }

        /// <summary>
        /// // Method to send an email
        /// </summary>
        /// <param name="mailRequestDto"></param>
        /// <returns></returns>
        public async Task<ResponseDto?> SendEmailAsync(MailRequestDto mailRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            { 
                ApiType = Sd.ApiType.POST,                             // Set the API type to POST
                Data = mailRequestDto,                                 // Pass the mail request data
                Url = Sd.OutboundAPIBase + "/api/Outbound/SendMail",   // Set the URL for the send mail API endpoint
                ContentType = Sd.ContentType.MultipartFormData         // Set the content type to multipart/form-data
            });
        }
    }
}
