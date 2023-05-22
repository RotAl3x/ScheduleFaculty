namespace ScheduleFaculty.Api.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    
    public string firstName { get; set; }
    
    public string lastName { get; set; }
}

public class RoleDto
{
    public string name { get; set; }
}