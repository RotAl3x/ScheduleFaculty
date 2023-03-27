using System.ComponentModel.DataAnnotations.Schema;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Entities;

public class FreeDaysClassroom
{
    public Guid Id { get; set; }
    
    [ForeignKey("Classroom")]
    public Guid ClassroomId { get; set; }
    public Classroom Classroom { get; set; }
    
    public DaysOfWeek DaysOfWeek{ get; set; }
}