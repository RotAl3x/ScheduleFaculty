namespace ScheduleFaculty.Api.DTOs;

public class StudyYearGroupDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int SemiGroup { get; set; }
    
    public int Group { get; set; }
    
    public Guid StudyProgramYearId { get; set; }
}