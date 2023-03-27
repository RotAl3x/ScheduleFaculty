namespace ScheduleFaculty.Core.Utils;

public class ClassroomResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public List<DaysOfWeek> DaysOfWeeks { get; set; }
}