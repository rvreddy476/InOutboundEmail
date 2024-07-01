using InboundOutbound.web.Models;

namespace InboundOutbound.web.Service.IService
{
    public interface IInboundService
    {
        
        Task<ResponseDto?> GetAllAsync();
    }
}
