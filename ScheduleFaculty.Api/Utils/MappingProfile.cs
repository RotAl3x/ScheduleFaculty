using AutoMapper;
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
        CreateMap<NumberOfGroupsOfYear,TotalGroupDto>().ReverseMap();
        CreateMap<StatusDto, Status>().ReverseMap();
    }
}