using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleFaculty.Core.Entities;

public class NumberOfGroupsOfYear
{
    public Guid Id { get; set; }
    
    [ForeignKey(("Program"))]
    public Guid StudyProgramYearId { get; set; }
    public StudyProgram Program { get; set; }
    
    public int NumberOfSemiGroups { get; set; }
    
    public int HowManySemiGroupsAreInAGroup { get; set; }
}