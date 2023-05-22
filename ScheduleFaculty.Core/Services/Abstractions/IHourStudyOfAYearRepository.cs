using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface IHourStudyOfAYearRepository
{
    Task<ActionResponse<HourStudyOfAYear>> GetById(Guid id);

    Task<ActionResponse<List<HourStudyOfAYear>>> GetByStudyYearSemiGroupId(Guid semiGroupId);

    Task<ActionResponse<List<HourStudyOfAYear>>> GetByCourseId(Guid courseId);

    Task<ActionResponse<List<HourStudyOfAYear>>> GetByHourTypeId(Guid hourTypeId);
    
    Task<ActionResponse<List<HourStudyOfAYear>>> GetByStudyProgramId(Guid studyProgramId);

    Task<ActionResponse<List<HourStudyOfAYear>>> GetByUserId(string userId);

    Task<ActionResponse<List<HourStudyOfAYear>>> GetByClassroomId(Guid classroomId);

    Task<ActionResponse<List<HourStudyOfAYear>>> GetAllHourStudyOfAYear();

    Task<ActionResponse<HourStudyOfAYear>> CreateHourStudyOfAYear( Guid courseHourTypeId,
        string userId,
        Guid classroomId, List<int> studyWeeks, int startTime,
        int endTime, DayOfWeek dayOfWeek);

    Task<ActionResponse<HourStudyOfAYear>> EditHourStudyOfAYear(Guid id, Guid semiGroup, Guid courseHourTypeId,
        string userId,
        Guid classroomId, List<int> studyWeeks, DateTime startTime, DateTime endTime);

    Task<ActionResponse> DeleteHourStudyOfAYear(Guid id);
}