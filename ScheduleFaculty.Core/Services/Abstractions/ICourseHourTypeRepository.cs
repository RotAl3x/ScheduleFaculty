using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface ICourseHourTypeRepository
{
    Task<ActionResponse<CourseHourType>> GetById(Guid id);
    
    Task<ActionResponse<List<CourseHourType>>> GetByCourseId(Guid courseId);

    Task<ActionResponse<List<CourseHourType>>> GetByHourType(Guid hourType);
    
    Task<ActionResponse<List<HourType>>> GetHourTypesByCourseId(Guid courseId);
    
    Task<ActionResponse<CourseHourType>> Create(Guid courseId,Guid hourTypeId);
    
    Task<ActionResponse<CourseHourType>> Edit(Guid id,Guid courseId,Guid hourTypeId);

    Task<ActionResponse<List<HourType>>> EditHourTypes(Guid courseId, List<Guid> hourTypeId);

    Task<ActionResponse> Delete(Guid id);

}