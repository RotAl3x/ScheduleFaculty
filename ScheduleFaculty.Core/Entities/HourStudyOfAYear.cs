using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleFaculty.Core.Entities;

public class HourStudyOfAYear
{
    public Guid Id { get; set; }

    [ForeignKey(("StudyYearGroup"))] 
    public List<Guid> SemiGroupId { get; set; } = new List<Guid>();
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
    
    public DayOfWeek DayOfWeek { get; set; }
    
    public int StartTime { get; set; }
    
    public int EndTime { get; set; }
    
    
}