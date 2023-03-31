namespace ScheduleFaculty.Core.Entities;

public class HourType
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    
    public int SemiGroupsPerHour { get; set; }
}