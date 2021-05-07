using System;
using AutoMapper;
using TaskService.Dto;
using TaskService.Models;

namespace TaskService.Profiles
{
    public class TextTaskProfile : Profile
    {
        public TextTaskProfile()
        {
            CreateMap<TextTaskDto, TextTask>()
                .ForMember(dest => dest.Duration,
                    opt => opt.MapFrom(src => GetDurationFromString(src.Duration)))
                .ReverseMap();
            CreateMap<TextTaskResult, TextTaskResultDto>().ReverseMap();
        }

        private TimeSpan GetDurationFromString(string duration)
        {
            if (TimeSpan.TryParse(duration, out var result))
            {
                return result;
            }

            throw new Exception("Could not convert duration to TimeSpan");
        }
    }
}
