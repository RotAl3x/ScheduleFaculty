namespace ScheduleFaculty.Core.Entities;

public class Status
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int Semester { get; set; }
    
    public bool IsActive { get; set; }
}