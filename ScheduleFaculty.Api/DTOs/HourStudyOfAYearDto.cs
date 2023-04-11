namespace ScheduleFaculty.Api.DTOs;

public class HourStudyOfAYearDto
{
    public Guid Id { get; set; }

    public List<Guid> SemiGroupsId { get; set; }

    public Guid CourseHourTypeId { get; set; }

    public string UserId { get; set; }

    public Guid ClassroomId { get; set; }

    public List<int> StudyWeeks { get; set; }
    
    public DayOfWeek DayOfWeek { get; set; }
    
    public int StartTime { get; set; }
    
    public int EndTime { get; set; }
}