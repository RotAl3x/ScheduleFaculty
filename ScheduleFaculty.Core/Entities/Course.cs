using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleFaculty.Core.Entities;

public class Course
{
    public Guid Id { get; set; }
    
    [ForeignKey(("StudyProgram"))]
    public Guid StudyProgramYearId { get; set; }
    public StudyProgram StudyProgram { get; set; }
    
    [ForeignKey(("ProfessorUser"))]
    public Guid ProfessorUserId { get; set; }
    public ApplicationUser ProfessorUser { get; set; }
    
    public string Name { get; set; }
    
    public string Abbreviation { get; set; }
    
    public int Semester { get; set; }
    
    public bool IsOptional { get; set; }
    
    
}