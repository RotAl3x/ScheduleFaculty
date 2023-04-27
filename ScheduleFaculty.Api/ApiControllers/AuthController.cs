using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.API.Utils;
using ScheduleFaculty.Core.Entities;


namespace ScheduleFaculty.Api.ApiControllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public AuthController(IMapper mapper, UserManager<ApplicationUser> userManager, IIdentityService identityService)
    {
        _userManager = userManager;
        _identityService = identityService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _identityService.Login(request);
        if (response.HasErrors())
        {
            return BadRequest(response.Errors);
        }

        return Ok(((ActionResponse<Session>)response).Item);
    }

    [HttpPost("register")]
    [Authorize(Roles = "Secretary,Professor,LabAssistant",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _identityService.Register(request);
        if (response.HasErrors())
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Item);
    }

    [HttpPost("changePassword")]
    [Authorize(Roles = "Secretary,Professor,LabAssistant",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound("Error to find user");
        }

        var response = await _identityService.ChangePassword(request, user);
        if (response.HasErrors())
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Item);
    }

    [HttpGet("getAllUsersByRole/{role}")]
    public async Task<IActionResult> GetAllUsers([FromRoute] string role)
    {
        var users = await _userManager.GetUsersInRoleAsync(role);

        if (!users.Any())
        {
            return NotFound("Error to find user with this role");
        }

        var response = _mapper.Map<List<UserDto>>(users);

        return Ok(response);
    }
}