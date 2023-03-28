using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Api.DTOs;

public class ClassroomDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public List<DaysOfWeek> DaysOfWeek { get; set; }
}