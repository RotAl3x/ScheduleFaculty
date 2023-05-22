using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.API.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Classroom, ClassroomDto>().ReverseMap();
        CreateMap<StudyProgram, StudyProgramDto>().ReverseMap();
        CreateMap<StudyYearGroup, StudyYearGroupDto>().ReverseMap();
        CreateMap<Status, StatusDto>().ReverseMap();
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<Course, CourseResponseDto>().ReverseMap();
        CreateMap<HourType, HourTypeDto>().ReverseMap();
        CreateMap<CourseHourType, CourseHourTypeDto>().ReverseMap();
        CreateMap<HourStudyOfAYear, HourStudyOfAYearDto>().ReverseMap();
        CreateMap<ApplicationUser, UserDto>().ReverseMap();
        CreateMap<CourseHourType, CourseHourTypeResponseDto>();
        CreateMap<HourStudyOfAYear, HourStudyOfAYearResponseDto>();
    }
}