using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface ICheckAvailabilityService
{
    Task<ActionResponse> ForClassroom(Guid classroomId, int startTime, int endTime, DayOfWeek dayOfWeek,
        List<int> studyWeeks, int semester);

    Task<ActionResponse<List<Guid>>> AvailableSemigroups(Guid courseHourTypeId, int startTime,
        int endTime, DayOfWeek dayOfWeek, List<int> studyWeeks);
}