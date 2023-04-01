namespace ScheduleFaculty.Api.DTOs;

public class HourTypeDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    
    public int SemiGroupsPerHour { get; set; }
    
    public bool NeedAllSemiGroups { get; set; }
}