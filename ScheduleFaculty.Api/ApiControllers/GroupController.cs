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

    [HttpPost]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> CreateGroup([FromBody] TotalGroupDto TotalGroupDto)
    {
        var group = await _studyYearGroupRepository.CreateStudyYearGroup(TotalGroupDto.StudyProgramId,
            TotalGroupDto.NumberOfSemiGroups, TotalGroupDto.HowManySemiGroupsAreInAGroup);
        if (group.HasErrors())
        {
            return BadRequest(group.Errors);
        }
        var response = _mapper.Map<List<StudyYearGroupDto>>(group.Item);
        return Ok(response);
    }
}