using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Api.ApiControllers;

[ApiController]
[Route("/api/course")]
public class CourseController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly IAssignedCourseUserRepository _assignedCourseUserRepository;
    private readonly ICourseHourTypeRepository _courseHourTypeRepository;
    private readonly IMapper _mapper;

    public CourseController(IAssignedCourseUserRepository assignedCourseUserRepository,ICourseHourTypeRepository courseHourTypeRepository,ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _courseHourTypeRepository = courseHourTypeRepository;
        _assignedCourseUserRepository = assignedCourseUserRepository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCourseById([FromRoute] Guid id)
    {
        var course = await _courseRepository.GetCourseById(id);
        var hourTypes = await _courseHourTypeRepository.GetHourTypesByCourseId(id);
        if (course.HasErrors())
        {
            return BadRequest(course.Errors);
        }
        
        if (hourTypes.HasErrors())
        {
            return BadRequest(hourTypes.Errors);
        }

        var response = _mapper.Map<CourseResponseDto>(course.Item);
        response.HourTypes = hourTypes.Item;
        return Ok(response);
    }

    [HttpGet("getByProfessorId/{id}")]
    public async Task<ActionResult> GetCourseByProfessorId([FromRoute] string id)
    {
        var courses = await _courseRepository.GetCoursesByProfessorId(id);
        if (courses.HasErrors())
        {
            return BadRequest(courses.Errors);
        }

        var response = _mapper.Map<List<CourseResponseDto>>(courses.Item);
        return Ok(response);
    }

    [HttpGet("getCourseByStudyProgramId/{id}")]
    public async Task<ActionResult> GetCourseByStudyProgramId([FromRoute] Guid id)
    {
        var courses = await _courseRepository.GetCoursesByStudyProgramId(id);
        if (courses.HasErrors())
        {
            return BadRequest(courses.Errors);
        }

        var response = _mapper.Map<List<CourseResponseDto>>(courses.Item);
        return Ok(response);
    }

    [HttpGet("getAllCourses")]
    public async Task<ActionResult> GetAllCourses()
    {
        var courses = await _courseRepository.GetAllCourses();
        if (courses.HasErrors())
        {
            return BadRequest(courses.Errors);
        }

        var response = _mapper.Map<List<CourseResponseDto>>(courses.Item);
        foreach (var res in response)
        {
            var hourTypes = await _courseHourTypeRepository.GetHourTypesByCourseId(res.Id);
            if (hourTypes.HasErrors())
            {
                res.HourTypes = new List<HourType>();
            }
            res.HourTypes = hourTypes.Item;
        }
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> CreateCourse([FromBody] CourseDto courseDto)
    {
        var course = await _courseRepository.CreateCourse(courseDto.StudyProgramYearId, courseDto.ProfessorUserId,
            courseDto.Name, courseDto.Abbreviation, courseDto.Semester, courseDto.IsOptional);
        
        if (course.HasErrors())
        {
            return BadRequest(course.Errors);
        }

        var assignedCourseUser =
            await _assignedCourseUserRepository.CreateAssignedCourseUser(course.Item.Id, course.Item.ProfessorUserId);
        
        if (assignedCourseUser.HasErrors())
        {
            return BadRequest(assignedCourseUser.Errors);
        }
        
        var hourTypes = new List<HourType>();

        foreach (var hourTypeId in courseDto.HourTypeIds)
        {
            var hourType = await _courseHourTypeRepository.Create(course.Item.Id, hourTypeId);
            hourTypes.Add(hourType.Item.HourType);
        }
        
        var response = _mapper.Map<CourseResponseDto>(course.Item);
        response.HourTypes = hourTypes;
        return Ok(response);
    }

    [HttpPatch("edit")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> EditCourse([FromBody] CourseDto courseDto)
    {
        var course = await _courseRepository.EditCourse(courseDto.Id,courseDto.StudyProgramYearId,courseDto.ProfessorUserId,courseDto.Name,courseDto.Abbreviation,courseDto.Semester,courseDto.IsOptional);
        if (course.HasErrors())
        {
            return BadRequest(course.Errors);
        }
        
        var hourTypes = await _courseHourTypeRepository.EditHourTypes(courseDto.Id, courseDto.HourTypeIds);
        if (hourTypes.HasErrors())
        {
            return BadRequest(hourTypes.Errors);
        }


        var response = _mapper.Map<CourseResponseDto>(course.Item);
        response.HourTypes = hourTypes.Item;
        return Ok(response);
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DeleteCourse([FromRoute] Guid id)
    {
        var course = await _courseRepository.DeleteCourse(id);
        if (course.HasErrors())
        {
            return BadRequest((course.Errors));
        }

        return Ok();
    }
}