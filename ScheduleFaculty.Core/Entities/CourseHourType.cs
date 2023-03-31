using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleFaculty.Core.Entities;

public class CourseHourType
{
    public Guid Id { get; set; }
    
    [ForeignKey(("Course"))]
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    
    [ForeignKey(("HourType"))]
    public Guid HourTypeId { get; set; }
    public HourType HourType { get; set; }
    
    public int TotalHours { get; set; }
}