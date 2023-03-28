using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Api.DTOs;

public class ClassroomDto
{

    public string Name { get; set; }
    
    public List<DaysOfWeek> DaysOfWeek { get; set; }
}