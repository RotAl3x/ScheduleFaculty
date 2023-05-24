using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleFaculty.Core.Entities;

public class AssignedCourseUser
{
    public Guid Id { get; set; }
    
    [ForeignKey(("ProfessorUser"))]
    public string ProfessorUserId { get; set; }
    public ApplicationUser ProfessorUser { get; set; }
    
    [ForeignKey(("Course"))]
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
}