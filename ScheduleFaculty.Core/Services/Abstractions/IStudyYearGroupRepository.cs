using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface IStudyYearGroupRepository
{
    Task<ActionResponse<List<StudyYearGroup>>> GetStudyYearGroup(Guid studyProgramId);

   Task SaveStudyYearGroup();
}