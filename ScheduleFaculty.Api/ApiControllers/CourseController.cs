using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;

namespace ScheduleFaculty.Api.ApiControllers;

[ApiController]
[Route("/api/course")]
public class CourseController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseController(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCourseById([FromRoute] Guid id)
    {
        var course = await _courseRepository.GetCourseById(id);
        if (course.HasErrors())
        {
            return BadRequest(course.Errors);
        }

        var response = _mapper.Map<CourseResponseDto>(course.Item);
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

        var response = _mapper.Map<CourseDto>(course.Item);
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

        var response = _mapper.Map<CourseDto>(course.Item);
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