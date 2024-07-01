

using InboundOutbound.web.Models;

namespace InboundOutbound.web.Service.IService
{
    public interface IOutboundService
    {
        Task<ResponseDto?> SendEmailAsync(MailRequestDto mailRequestDto);
        Task<ResponseDto?> GetAllAsync();
    }
}
