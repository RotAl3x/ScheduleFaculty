using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Services.Abstractions;

namespace ScheduleFaculty.Api.ApiControllers;


[ApiController]
[Route("/api/group")]
public class GroupController : ControllerBase
{
    private readonly IStudyYearGroupRepository _studyYearGroupRepository;
    private readonly IMapper _mapper;

    public GroupController(IStudyYearGroupRepository studyYearGroupRepository, IMapper mapper)
    {
        _mapper = mapper;
        _studyYearGroupRepository = studyYearGroupRepository;
    }
    
    [HttpGet("{studyProgramId}")]
    public async Task<ActionResult> GetStudyYearGroup([FromRoute] Guid studyProgramId)
    {
        var studyYearGroup = await _studyYearGroupRepository.GetStudyYearGroup(studyProgramId);
        if (studyYearGroup.HasErrors())
        {
            return BadRequest(studyYearGroup.Errors);
        }

        var response = _mapper.Map<List<StudyYearGroupDto>>(studyYearGroup.Item);
        return Ok(response);

    }
}