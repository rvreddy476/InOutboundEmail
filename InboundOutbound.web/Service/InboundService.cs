using InboundOutbound.web.Models;
using InboundOutbound.web.Service.IService;
using InboundOutbound.web.Utility;

namespace InboundOutbound.web.Service
{
    public class InboundService : IInboundService
    {
        private readonly IBaseService _baseService;
        public InboundService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        /// <summary>
        /// /// <summary>
        /// Method to retrieve all incoming emails
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDto?> GetAllAsync()
        {
            // // Call the base service to send a GET request to retrieve all inbound emails
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = Sd.ApiType.GET,
                Url = Sd.InboundAPIBase + "/api/inbound/receive"
            });
        }

    }
}
