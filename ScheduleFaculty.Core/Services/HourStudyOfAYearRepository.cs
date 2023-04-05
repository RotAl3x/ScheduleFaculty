using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class HourStudyOfAYearRepository:IHourStudyOfAYearRepository
{
    public Task<ActionResponse<HourStudyOfAYear>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<HourStudyOfAYear>>> GetByStudyYearGroupId(Guid semiGroupId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<HourStudyOfAYear>>> GetByCourseId(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<HourStudyOfAYear>>> GetByHourTypeId(Guid hourTypeId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<HourStudyOfAYear>>> GetByUserId(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<HourStudyOfAYear>>> GetByClassroomId(Guid classroomId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<HourStudyOfAYear>>> GetAllHourStudyOfAYear()
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<HourStudyOfAYear>> CreateHourStudyOfAYear(Guid semiGroup, Guid courseHourTypeId, string userId, Guid classroomId, List<int> studyWeeks,
        DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<HourStudyOfAYear>> EditHourStudyOfAYear(Guid id, Guid semiGroup, Guid courseHourTypeId, string userId, Guid classroomId,
        List<int> studyWeeks, DateTime startTime, DateTime endTime)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<HourStudyOfAYear>> DeleteHourStudyOfAYear(Guid id)
    {
        throw new NotImplementedException();
    }
}