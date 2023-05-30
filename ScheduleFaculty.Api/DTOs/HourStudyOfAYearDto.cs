using ScheduleFaculty.Core.Entities;

namespace ScheduleFaculty.Api.DTOs;

public class HourStudyOfAYearDto
{
    public Guid? Id { get; set; }

    public List<Guid>? SemiGroupsId { get; set; }

    public Guid CourseHourTypeId { get; set; }

    public string UserId { get; set; }

    public Guid ClassroomId { get; set; }

    public List<int> StudyWeeks { get; set; }
    
    public DayOfWeek DayOfWeek { get; set; }
    
    public int StartTime { get; set; }
    
    public int EndTime { get; set; }
}

public class HourStudyOfAYearResponseDto
{
    public Guid Id { get; set; }

    public List<StudyYearGroup> SemiGroups { get; set; }

    public CourseHourType CourseHourType { get; set; }

    public UserDto User { get; set; }

    public Classroom Classroom { get; set; }

    public List<int> StudyWeeks { get; set; }
    
    public DayOfWeek DayOfWeek { get; set; }
    
    public int StartTime { get; set; }
    
    public int EndTime { get; set; }
}

public class HourStudyOfAYearMACAddress
{
    public string CourseAbbreviation { get; set; }
    
    public string StudyProgramName { get; set; }
    
    public string UserName { get; set; }
    
    public string DayOfWeek { get; set; }
    
    public int StartTime { get; set; }
    
    public int EndTime { get; set; }
}

public class HourStudyOfAYearMACAddressResponseDto
{
    public string ClassroomName { get; set; }
    
    public List<HourStudyOfAYearMACAddress> HourStudyOfAYearMacAddresses { get; set; }

}