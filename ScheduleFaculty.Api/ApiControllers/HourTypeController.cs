using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;

namespace ScheduleFaculty.Api.ApiControllers;

[ApiController]
[Route("/api/hourType")]
public class HourTypeController: ControllerBase
{
    private readonly IHourTypeRepository _hourTypeRepository;
    private readonly IMapper _mapper;

    public HourTypeController(IHourTypeRepository hourTypeRepository, IMapper mapper)
    {
        _hourTypeRepository = hourTypeRepository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetHourType([FromRoute] Guid id)
    {
        var hourType = await _hourTypeRepository.GetHourType(id);
        if (hourType.HasErrors())
        {
            return BadRequest(hourType.Errors);
        }

        var response = _mapper.Map<HourTypeDto>(hourType.Item);
        return Ok(response);
    }
    
    [HttpGet("allHourType")]
    public async Task<ActionResult> GetAllHourType()
    {
        var hourTypes = await _hourTypeRepository.GetAllHourType();

        var response = _mapper.Map<List<HourTypeDto>>(hourTypes.Item);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> CreateStatus([FromBody] string name,int semiGroupsPerHour,bool needAllSemiGroup)
    {
        var createHourType = await _hourTypeRepository.CreateHourType(name, semiGroupsPerHour, needAllSemiGroup);
        if (createHourType.HasErrors())
        {
            return BadRequest(createHourType.Errors);
        }

        var response = _mapper.Map<HourTypeDto>(createHourType.Item);
        return Ok(response);
    }

    [HttpPatch("edit")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> EditStatus([FromBody] HourType hourType)
    {
        var editHourType = await _hourTypeRepository.EditHourType(hourType);
        if (editHourType.HasErrors())
        {
            return BadRequest(editHourType.Errors);
        }

        var response = _mapper.Map<HourTypeDto>(editHourType.Item);
        return Ok(response);
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DeleteStatus([FromRoute] Guid id)
    {
        var deleteHourType = await _hourTypeRepository.DeleteHourType(id);
        if (deleteHourType.HasErrors())
        {
            return BadRequest(deleteHourType.Errors);
        }

        var response = _mapper.Map<HourTypeDto>(deleteHourType);
        return Ok(response);
    }
}