using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Services.Abstractions;

namespace ScheduleFaculty.Api.ApiControllers;

[ApiController]
[Route("/api/courseHourType")]
public class CourseHourTypeController : ControllerBase
{
    private readonly ICourseHourTypeRepository _courseHourTypeRepository;
    private readonly IMapper _mapper;

    public CourseHourTypeController(ICourseHourTypeRepository courseHourTypeRepository, IMapper mapper)
    {
        _courseHourTypeRepository = courseHourTypeRepository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById([FromRoute] Guid id)
    {
        var courseHourType = await _courseHourTypeRepository.GetById(id);
        if (courseHourType.HasErrors())
        {
            return BadRequest(courseHourType.Errors);
        }

        var response = _mapper.Map<CourseHourTypeDto>(courseHourType.Item);
        return Ok(response);
    }

    [HttpGet("byCourseId/{id}")]
    public async Task<ActionResult> GetByCourseId([FromRoute] Guid id)
    {
        var courseHourType = await _courseHourTypeRepository.GetByCourseId(id);
        if (courseHourType.HasErrors())
        {
            return BadRequest(courseHourType.Errors);
        }

        var response = _mapper.Map<CourseHourTypeDto>(courseHourType.Item);
        return Ok(response);
    }

    [HttpGet("byHourTypeId/{id}")]
    public async Task<ActionResult> GetByHourTypeId([FromRoute] Guid id)
    {
        var courseHourType = await _courseHourTypeRepository.GetByHourType(id);
        if (courseHourType.HasErrors())
        {
            return BadRequest(courseHourType.Errors);
        }

        var response = _mapper.Map<CourseHourTypeDto>(courseHourType.Item);
        return Ok(response);
    }

    [HttpPost("create")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Create([FromBody] CourseHourTypeDto courseHourTypeDto)
    {
        var courseHourType = await _courseHourTypeRepository.Create(courseHourTypeDto.CourseId,
            courseHourTypeDto.HourTypeId, courseHourTypeDto.TotalHours);
        if (courseHourType.HasErrors())
        {
            return BadRequest(courseHourType.Errors);
        }

        var response = _mapper.Map<CourseHourTypeDto>(courseHourType.Item);
        return Ok(response);
    }

    [HttpPatch("edit")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Edit([FromBody] CourseHourTypeDto courseHourTypeDto)
    {
        var courseHourType = await _courseHourTypeRepository.Edit(courseHourTypeDto.Id, courseHourTypeDto.CourseId,
            courseHourTypeDto.HourTypeId, courseHourTypeDto.TotalHours);
        if (courseHourType.HasErrors())
        {
            return BadRequest(courseHourType.Errors);
        }

        var response = _mapper.Map<CourseHourTypeDto>(courseHourType.Item);
        return Ok(response);
    }

    [HttpDelete("delete")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var courseHourType = await _courseHourTypeRepository.Delete(id);
        if (courseHourType.HasErrors())
        {
            return BadRequest(courseHourType.Errors);
        }

        return Ok();
    }
}