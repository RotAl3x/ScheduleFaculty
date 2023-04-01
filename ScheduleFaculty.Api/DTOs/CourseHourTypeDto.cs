namespace ScheduleFaculty.Api.DTOs;

public class CourseHourTypeDto
{
    public Guid Id { get; set; }
    
    public Guid CourseId { get; set; }
    
    public Guid HourTypeId { get; set; }

    public int TotalHours { get; set; }
}