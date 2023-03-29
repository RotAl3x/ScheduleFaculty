using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleFaculty.Core.Entities;

public class StudyYearGroup
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int SemiGroup { get; set; }
    
    public int Group { get; set; }
    
    [ForeignKey(("Program"))]
    public Guid StudyProgramYearId { get; set; }
    public StudyProgram Program { get; set; }
}