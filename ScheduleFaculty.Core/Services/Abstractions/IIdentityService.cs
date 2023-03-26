using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface IIdentityService
{
    Task<ActionResponse> Login(LoginRequest request);
    
    Task<ActionResponse<string>> Register(RegisterRequest request, string role);
    
    Task<ActionResponse<string>> ChangePassword(ChangePasswordRequest request,ApplicationUser user);
}