using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Entities;

public class Classroom
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public List<DaysOfWeek> DaysOfWeek{ get; set; }
    
}