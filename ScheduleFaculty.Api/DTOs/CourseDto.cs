namespace ScheduleFaculty.Api.DTOs;

public class CourseDto
{
    public Guid Id { get; set; }
    
    public Guid StudyProgramYearId { get; set; }
    
    public string ProfessorUserId { get; set; }
    
    public string Name { get; set; }
    
    public string Abbreviation { get; set; }
    
    public int Semester { get; set; }
    
    public bool IsOptional { get; set; }
}