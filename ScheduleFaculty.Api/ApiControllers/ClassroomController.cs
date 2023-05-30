using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;

namespace ScheduleFaculty.Api.ApiControllers;


[ApiController]
[Route("/api/classroom")]
public class ClassroomController : ControllerBase
{
    private readonly IClassroomRepository _classroomRepository;
    private readonly IMapper _mapper;

    public ClassroomController(IClassroomRepository classroomRepository,IMapper mapper)
    {
        _classroomRepository = classroomRepository;
        _mapper = mapper;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetClassroomById([FromRoute] Guid id)
    {
        var classroom = await _classroomRepository.GetClassroom(id);
        if (classroom.HasErrors())
        {
            return BadRequest(classroom.Errors);
        }

        var response = _mapper.Map<ClassroomDto>(classroom.Item);
        return Ok(response);

    }

    [HttpGet("getAll")]
    public async Task<ActionResult> GetAllClassroom()
    {
        var classrooms = await _classroomRepository.GetAllClassrooms();
        var response = _mapper.Map<List<ClassroomDto>>(classrooms.Item);
        for (var i = 0; i < response.Count; i++)
        {
            for (var j = 0; j < response[i].DaysOfWeek.Count; j++)
            {
                response[i].DaysOfWeek[j] = response[i].DaysOfWeek[j].ToString();
            }
        }

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> CreateClassroom([FromBody] ClassroomDto classroomDto)
    {
        var freeDays = new List<DayOfWeek>();
        foreach (var day in classroomDto.DaysOfWeek)
        {
            Enum.TryParse(day, out DayOfWeek free);
            freeDays.Add(free);
        }
        var classroom = await _classroomRepository.CreateClassroom(classroomDto.Name,freeDays,classroomDto.MACAddress );
        if (classroom.HasErrors())
        {
            return BadRequest(classroom.Errors);
        }

        var response = _mapper.Map<ClassroomDto>(classroom.Item);
        return Ok(response);
    }

    [HttpPatch("edit")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> EditClassroom([FromBody] ClassroomDto classroomDto)
    {
        var freeDays = new List<DayOfWeek>();
        foreach (var day in classroomDto.DaysOfWeek)
        {
            Enum.TryParse(day, out DayOfWeek free);
            freeDays.Add(free);
        }
        var classroom = await _classroomRepository.EditClassroom(classroomDto.Id,classroomDto.Name, freeDays,classroomDto.MACAddress);
        if (classroom.HasErrors())
        {
            return BadRequest(classroom.Errors);
        }

        var response = _mapper.Map<ClassroomDto>(classroom.Item);
        return Ok(response);
    }
    
    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DeleteClassroom([FromRoute] Guid id)
    {
        var classroom = await _classroomRepository.DeleteClassroom(id);
        if (classroom.HasErrors())
        {
            return BadRequest(classroom.Errors);
        }
        return Ok();
    }
}