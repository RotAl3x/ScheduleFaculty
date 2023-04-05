using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleFaculty.Core.Entities;

public class HourStudyOfAYear
{
    public Guid Id { get; set; }

    [ForeignKey(("StudyYearGroup"))] 
    public Guid SemiGroupId { get; set; }
    public StudyYearGroup StudyYearGroup { get; set; }
    
    [ForeignKey(("CourseHourType"))] 
    public Guid CourseHourTypeId { get; set; }
    public CourseHourType CourseHourType { get; set; }
    
    [ForeignKey(("User"))] 
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    
    [ForeignKey(("Classroom"))] 
    public Guid ClassroomId { get; set; }
    public Classroom Classroom { get; set; }
    
    public List<int> StudyWeeks { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    
}