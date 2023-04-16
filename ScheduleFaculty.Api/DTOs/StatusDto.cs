namespace ScheduleFaculty.Api.DTOs;

public class StatusDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int Semester { get; set; }
    
    public bool IsActive { get; set; }
}