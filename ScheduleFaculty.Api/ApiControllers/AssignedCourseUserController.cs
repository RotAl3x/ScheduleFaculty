using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Services.Abstractions;

namespace ScheduleFaculty.Api.ApiControllers;


[ApiController]
[Route("/api/assignedCourseUser")]
public class AssignedCourseUserController: ControllerBase
{
    private readonly IAssignedCourseUserRepository _assignedCourseUser;
    private readonly IMapper _mapper;

    public AssignedCourseUserController(IAssignedCourseUserRepository assignedCourseUser,IMapper mapper)
    {
        _assignedCourseUser = assignedCourseUser;
        _mapper = mapper;
    }
    
    [HttpGet("getAll")]
    public async Task<ActionResult> GetAllAssignedCourseUser()
    {
        var response = await _assignedCourseUser.GetAllAssignedCourseUser();
        return Ok(response.Item);
    }
    
    [HttpGet("getByCourseId/{id}")]
    public async Task<ActionResult> GetAssignedCourseUserByCourseId([FromRoute] Guid id)
    {
        var assignedCourseUser = await _assignedCourseUser.GetAssignedCourseUserByCourseId(id);
        if (assignedCourseUser.HasErrors())
        {
            return BadRequest(assignedCourseUser.Errors);
        }
        return Ok(assignedCourseUser.Item);
    }
    
    [HttpGet("getByUserId/{id}")]
    public async Task<ActionResult> GetAssignedCourseUserByProfessorId([FromRoute] string id)
    {
        var assignedCourseUser = await _assignedCourseUser.GetAssignedCourseUsersByProfessorId(id);
        if (assignedCourseUser.HasErrors())
        {
            return BadRequest(assignedCourseUser.Errors);
        }
        return Ok(assignedCourseUser.Item);
    }
    
    [HttpPost("create")]
    [Authorize(Roles = "Secretary,Professor,LabAssistant",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> CreateAssignedCourseUser([FromBody] AssignedCourseUserDto assignedCourseUserDto)
    {
        var assignedCourseUser = await _assignedCourseUser.CreateAssignedCourseUser(assignedCourseUserDto.CourseId,
            assignedCourseUserDto.ProfessorUserId);
        if (assignedCourseUser.HasErrors())
        {
            return BadRequest(assignedCourseUser.Errors);
        }
        return Ok(assignedCourseUser.Item);
    }
    
    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Secretary,Professor,LabAssistant",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DeleteAssignedCourseUser([FromRoute] Guid id)
    {
        var assignedCourseUser = await _assignedCourseUser.DeleteAssignedCourseUser(id);
        if (assignedCourseUser.HasErrors())
        {
            return BadRequest(assignedCourseUser.Errors);
        }
        return Ok();
    }
}