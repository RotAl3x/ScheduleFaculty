using System.ComponentModel.DataAnnotations;

namespace ScheduleFaculty.Core.Utils;

public class LoginRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}