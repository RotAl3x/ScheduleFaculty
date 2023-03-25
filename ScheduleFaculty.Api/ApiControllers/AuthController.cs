using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;
using Microsoft.AspNetCore.Mvc;


namespace ScheduleFaculty.Api.ApiControllers;

[ApiController]
[Route("/api/auth")]

public class AuthController: ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
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

        return Ok(((ActionResponse<Session>) response).Item);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _identityService.Register(request, "Secretary");
        if (response.HasErrors())
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Item);
    }
}