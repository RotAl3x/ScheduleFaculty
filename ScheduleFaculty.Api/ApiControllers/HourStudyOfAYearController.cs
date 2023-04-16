using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Services.Abstractions;

namespace ScheduleFaculty.Api.ApiControllers;
[ApiController]
[Route("/api/hourStudyOfAYear")]
public class HourStudyOfAYearController: ControllerBase
{
    private readonly IHourStudyOfAYearRepository _hourStudyOfAYearRepository;
    private readonly IMapper _mapper;

    public HourStudyOfAYearController(IHourStudyOfAYearRepository hourStudyOfAYearRepository, IMapper mapper)
    {
        _hourStudyOfAYearRepository = hourStudyOfAYearRepository;
        _mapper = mapper;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById([FromRoute] Guid id)
    {
        var hoursStudy = await _hourStudyOfAYearRepository.GetById(id);
        if (hoursStudy.HasErrors())
        {
            return BadRequest(hoursStudy.Errors);
        }

        var response = _mapper.Map<HourStudyOfAYearDto>(hoursStudy.Item);
        return Ok(response);
    }
    
    [HttpGet("semiGroupId/{id}")]
    public async Task<ActionResult> GetByStudyYearSemiGroupId([FromRoute] Guid id)
    {
        var hoursStudy = await _hourStudyOfAYearRepository.GetByStudyYearSemiGroupId(id);
        if (hoursStudy.HasErrors())
        {
            return BadRequest(hoursStudy.Errors);
        }

        var response = _mapper.Map<HourStudyOfAYearDto>(hoursStudy.Item);
        return Ok(response);
    }
    
    [HttpGet("courseId/{id}")]
    public async Task<ActionResult> GetByCourseId([FromRoute] Guid id)
    {
        var hoursStudy = await _hourStudyOfAYearRepository.GetByCourseId(id);
        if (hoursStudy.HasErrors())
        {
            return BadRequest(hoursStudy.Errors);
        }

        var response = _mapper.Map<HourStudyOfAYearDto>(hoursStudy.Item);
        return Ok(response);
    }
    
    [HttpGet("hourTypeId/{id}")]
    public async Task<ActionResult> GetByHourTypeId([FromRoute] Guid id)
    {
        var hoursStudy = await _hourStudyOfAYearRepository.GetByHourTypeId(id);
        if (hoursStudy.HasErrors())
        {
            return BadRequest(hoursStudy.Errors);
        }

        var response = _mapper.Map<HourStudyOfAYearDto>(hoursStudy.Item);
        return Ok(response);
    }
    
        
    [HttpGet("userId/{id}")]
    public async Task<ActionResult> GetByUserId([FromRoute] string id)
    {
        var hoursStudy = await _hourStudyOfAYearRepository.GetByUserId(id);
        if (hoursStudy.HasErrors())
        {
            return BadRequest(hoursStudy.Errors);
        }

        var response = _mapper.Map<HourStudyOfAYearDto>(hoursStudy.Item);
        return Ok(response);
    }
    
    [HttpGet("classroomId/{id}")]
    public async Task<ActionResult> GetByClassroomId([FromRoute] Guid id)
    {
        var hoursStudy = await _hourStudyOfAYearRepository.GetByClassroomId(id);
        if (hoursStudy.HasErrors())
        {
            return BadRequest(hoursStudy.Errors);
        }

        var response = _mapper.Map<HourStudyOfAYearDto>(hoursStudy.Item);
        return Ok(response);
    }
    
    [HttpGet("getAll")]
    public async Task<ActionResult> GetAllHourStudyOfAYear()
    {
        var hoursStudy = await _hourStudyOfAYearRepository.GetAllHourStudyOfAYear();
        if (hoursStudy.HasErrors())
        {
            return BadRequest(hoursStudy.Errors);
        }

        var response = _mapper.Map<List<HourStudyOfAYearDto>>(hoursStudy.Item);
        return Ok(response);
    }
    
    [HttpPost]
    [Authorize(Roles = "Secretary,Professor,LabAssistant",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> CreateStatus([FromBody] HourStudyOfAYearDto hourStudyOfAYearDto)
    {
        var createHourStudy = await _hourStudyOfAYearRepository.CreateHourStudyOfAYear(
            hourStudyOfAYearDto.CourseHourTypeId, hourStudyOfAYearDto.UserId, hourStudyOfAYearDto.ClassroomId,
            hourStudyOfAYearDto.StudyWeeks, hourStudyOfAYearDto.StartTime, hourStudyOfAYearDto.EndTime,
            hourStudyOfAYearDto.DayOfWeek);
        if (createHourStudy.HasErrors())
        {
            return BadRequest(createHourStudy.Errors);
        }

        var response = _mapper.Map<HourStudyOfAYearDto>(createHourStudy.Item);
        return Ok(response);
    }
    
    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Secretary,Professor,LabAssistant",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DeleteStatus([FromRoute] Guid id)
    {
        var delete = await _hourStudyOfAYearRepository.DeleteHourStudyOfAYear(id);
        if (delete.HasErrors())
        {
            return BadRequest(delete.Errors);
        }

        return Ok();
    }
}