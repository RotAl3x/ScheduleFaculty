using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface IGroupsOfAStudyHourRepository
{
    Task<ActionResponse<List<StudyYearGroup>>> GetByHourStudyId(Guid id);
}