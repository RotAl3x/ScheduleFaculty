namespace ScheduleFaculty.Api.DTOs;

public class AssignedCourseUserDto
{
    public string ProfessorUserId { get; set; }

    public Guid CourseId { get; set; }

}