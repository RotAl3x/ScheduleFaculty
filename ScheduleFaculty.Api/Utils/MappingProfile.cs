using AutoMapper;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Entities;

namespace ScheduleFaculty.API.Utils;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Classroom, ClassroomDto>().ReverseMap();
        CreateMap<StudyProgram, StudyProgramDto>().ReverseMap();
        CreateMap<StudyYearGroup, StudyYearGroupDto>().ReverseMap();
    }
}