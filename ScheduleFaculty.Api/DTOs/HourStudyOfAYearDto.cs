namespace ScheduleFaculty.Api.DTOs;

public class HourStudyOfAYearDto
{
    public Guid Id { get; set; }

    public Guid SemiGroupId { get; set; }

    public Guid CourseHourTypeId { get; set; }

    public string UserId { get; set; }

    public Guid ClassroomId { get; set; }

    public List<int> StudyWeeks { get; set; }
    
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}