using AutoMapper;
using Frontend.Dto;
using Frontend.Entities;

namespace Frontend.Mappers
{
    public class V2TMapper : Profile
    {
        public V2TMapper()
        {
            CreateMap<SpeechToTextRequestDto, SpeechToTextRequest>().ReverseMap();
        }
    }
}