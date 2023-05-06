namespace ScheduleFaculty.Api.DTOs;

public class StudyProgramDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int Year { get; set; }
    
    public int WeeksInASemester { get; set; }
    
    public int NumberOfSemiGroups { get; set; }
    
    public int HowManySemiGroupsAreInAGroup { get; set; }
}