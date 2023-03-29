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
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetStudyYearGroup([FromRoute] Guid id)
    {
        var studyYearGroup = await _studyYearGroupRepository.GetStudyYearGroup(id);
        if (studyYearGroup.HasErrors())
        {
            return BadRequest(studyYearGroup.Errors);
        }

        var response = _mapper.Map<List<StudyYearGroupDto>>(studyYearGroup.Item);
        return Ok(response);

    } 
    [HttpGet("numberOfGroups/{id}")]
    public async Task<ActionResult> GetStudyYearNumberOfGroups([FromRoute] Guid id)
    {
        var studyYearNumberOfGroups = await _studyYearGroupRepository.GetStudyYearNumberOfGroups(id);
        if (studyYearNumberOfGroups.HasErrors())
        {
            return BadRequest(studyYearNumberOfGroups.Errors);
        }

        var response = _mapper.Map<TotalGroupDto>(studyYearNumberOfGroups.Item);
        return Ok(response);

    }

    [HttpPost]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> CreateGroup([FromBody] TotalGroupDto totalGroupDto)
    {
        var group = await _studyYearGroupRepository.CreateStudyYearGroup(totalGroupDto.StudyProgramId,
            totalGroupDto.NumberOfSemiGroups, totalGroupDto.HowManySemiGroupsAreInAGroup);
        if (group.HasErrors())
        {
            return BadRequest(group.Errors);
        }
        var response = _mapper.Map<List<StudyYearGroupDto>>(group.Item);
        return Ok(response);
    }
    
    [HttpPatch("edit")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> EditStudyYearGroup([FromBody] TotalGroupDto totalGroupDto)
    {
        var studyYearGroup = await _studyYearGroupRepository.EditStudyYearGroup(totalGroupDto.StudyProgramId,totalGroupDto.NumberOfSemiGroups,totalGroupDto.HowManySemiGroupsAreInAGroup);
        if (studyYearGroup.HasErrors())
        {
            return BadRequest(studyYearGroup.Errors);
        }

        var response = _mapper.Map<List<StudyYearGroupDto>>(studyYearGroup.Item);
        return Ok(response);
    }
    
    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DeleteStudyYearGroup([FromRoute] Guid id)
    {
        var studyYearGroup = await _studyYearGroupRepository.DeleteStudyYearGroup(id);
        if (studyYearGroup.HasErrors())
        {
            return BadRequest(studyYearGroup.Errors);
        }
        return Ok();
    }
}