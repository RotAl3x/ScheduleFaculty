using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;

namespace ScheduleFaculty.Api.ApiControllers;


[ApiController]
[Route("/api/status")]
public class StatusController : ControllerBase
{
    private readonly IStatusRepository _statusRepository;
    private readonly IMapper _mapper;

    public StatusController(IStatusRepository statusRepository, IMapper mapper)
    {
        _statusRepository = statusRepository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetStatus([FromRoute] Guid id)
    {
        var status = await _statusRepository.GetStatus(id);
        if (status.HasErrors())
        {
            return BadRequest(status.Errors);
        }

        var response = _mapper.Map<StatusDto>(status.Item);
        return Ok(response);
    }

    [HttpGet("activeStatus")]
    public async Task<ActionResult> GetActiveStatus()
    {
        var activeStatus = await _statusRepository.GetActiveStatus();
        if (activeStatus.HasErrors())
        {
            return BadRequest(activeStatus.Errors);
        }

        var response = _mapper.Map<StatusDto>(activeStatus.Item);
        return Ok(response);
    }

    [HttpGet("allStatuses")]
    public async Task<ActionResult> GetAllStatuses()
    {
        var allStatuses = await _statusRepository.GetAllStatuses();
        if (allStatuses.HasErrors())
        {
            return BadRequest(allStatuses.Errors);
        }

        var response = _mapper.Map<List<StatusDto>>(allStatuses.Item);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> CreateStatus([FromBody] string name)
    {
        var createStatus = await _statusRepository.CreateStatus(name);
        if (createStatus.HasErrors())
        {
            return BadRequest(createStatus.Errors);
        }

        var response = _mapper.Map<StatusDto>(createStatus.Item);
        return Ok(response);
    }

    [HttpPatch("edit")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> EditStatus([FromBody] StatusDto statusDto)
    {
        var editStatus = await _statusRepository.EditStatus(statusDto.Id, statusDto.Name, statusDto.IsActive, statusDto.Semester);
        if (editStatus.HasErrors())
        {
            return BadRequest(editStatus.Errors);
        }

        var response = _mapper.Map<StatusDto>(editStatus.Item);
        return Ok(response);
    }
    
    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DeleteStatus([FromRoute] Guid id)
    {
        var deleteStatus = await _statusRepository.DeleteStatus(id);
        if (deleteStatus.HasErrors())
        {
            return BadRequest(deleteStatus.Errors);
        }

        var response = _mapper.Map<StatusDto>(deleteStatus);
        return Ok(response);
    }

}