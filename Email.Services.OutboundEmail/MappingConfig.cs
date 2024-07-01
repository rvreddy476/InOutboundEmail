using AutoMapper;
using Email.Services.OutboundEmail.Models;
using Email.Services.OutboundEmail.Models.DTO;

namespace OutboundEmailServices
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<MailRequest, MailRequestDto>();
                config.CreateMap<MailRequestDto, MailRequest>();
            });
            return mappingConfig;
        }
    }
}
