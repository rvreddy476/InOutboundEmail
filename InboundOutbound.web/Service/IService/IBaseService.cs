using InboundOutbound.web.Models;

namespace InboundOutbound.web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto);
    }
}
