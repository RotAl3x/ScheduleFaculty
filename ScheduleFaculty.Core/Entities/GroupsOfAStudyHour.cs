using System.ComponentModel.DataAnnotations.Schema;

namespace ScheduleFaculty.Core.Entities;

public class GroupsOfAStudyHour
{
    public Guid Id { get; set; }
    
    [ForeignKey(("StudyYearGroup"))] 
    public Guid SemiGroupId { get; set; } = new Guid();
    public StudyYearGroup StudyYearGroup { get; set; }
    
    [ForeignKey(("HourStudyOfAYear"))] 
    public Guid HourStudyOfAYearId { get; set; } = new Guid();
    public HourStudyOfAYear HourStudyOfAYear { get; set; }
}