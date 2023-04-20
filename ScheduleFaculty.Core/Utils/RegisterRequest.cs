using System.ComponentModel.DataAnnotations;

namespace ScheduleFaculty.Core.Utils;

public class RegisterRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required]
    public string FirstName { get; set; }
        
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string role { get; set; }
}