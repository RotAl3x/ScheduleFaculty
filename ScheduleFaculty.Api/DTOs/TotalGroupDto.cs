namespace ScheduleFaculty.Api.DTOs;

public class TotalGroupDto
{
    //public Guid Id { get; set; }
    
    public Guid StudyProgramId { get; set; }
    
    public int NumberOfSemiGroups { get; set; }
    
    public int HowManySemiGroupsAreInAGroup { get; set; }
}