using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.API.Utils;
using ScheduleFaculty.Core.Entities;


namespace ScheduleFaculty.Api.ApiControllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(UserManager<ApplicationUser> userManager, IIdentityService identityService)
    {
        _userManager = userManager;
        _identityService = identityService;
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

    [HttpPost("registerSecretary")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RegisterSecretary([FromBody] RegisterRequest request)
    {
        var response = await _identityService.Register(request, "Secretary");
        if (response.HasErrors())
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Item);
    }
    
    [HttpPost("registerProfessor")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RegisterProfessor([FromBody] RegisterRequest request)
    {
        var response = await _identityService.Register(request, "Professor");
        if (response.HasErrors())
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Item);
    }
    
    [HttpPost("registerLabAssistant")]
    [Authorize(Roles = "Secretary,Professor,LabAssistant",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RegisterLabAssistant([FromBody] RegisterRequest request)
    {
        var response = await _identityService.Register(request, "LabAssistant");
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
}