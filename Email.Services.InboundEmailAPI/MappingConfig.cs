
using AutoMapper;
using Email.Services.InboundEmailAPI.Models;

namespace Email.Services.InboundEmailAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<InboundEmail, InboundEmail>();
            });
            return mappingConfig;
        }
    }
}
