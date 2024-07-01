using AutoMapper;
using Email.Services.EmailTrailAPI.Models.DTO;

namespace Email.Services.EmailTrailAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<MailSettingsDto, MergedEmailDto>()
                .ForMember(dest => dest.TicketId, u => u.MapFrom(src => src.UniqueId))
                .ForMember(dest => dest.ToEmail, u => u.MapFrom(src => src.ToEmail))
                .ForMember(dest => dest.Body, u => u.MapFrom(src => src.Body))
                .ForMember(dest => dest.Subject, u => u.MapFrom(src => src.Subject))
                .ForMember(dest => dest.Attachments, u => u.MapFrom(src => src.Attachments));
                
                config.CreateMap<InboundEmailDto, MergedEmailDto>()
                .ForMember(dest => dest.TicketId, u => u.MapFrom(src => src.TicketId))
                .ForMember(dest => dest.From, u => u.MapFrom(src => src.From))
                .ForMember(dest => dest.Body, u => u.MapFrom(src => src.Body))
                .ForMember(dest => dest.Subject, u => u.MapFrom(src => src.Subject))
                .ForMember(dest => dest.Attachments, u => u.MapFrom(src => src.Attachments));

            });
            return mappingConfig;
        }
    }
}
