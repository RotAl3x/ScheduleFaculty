using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface ICourseHourTypeRepository
{
    Task<ActionResponse<CourseHourType>> GetById(Guid id);
    
    Task<ActionResponse<CourseHourType>> GetByCourseId(Guid courseId);

    Task<ActionResponse<CourseHourType>> GetByHourType(Guid hourType);
    
    Task<ActionResponse<CourseHourType>> Create(Guid courseId,Guid hourTypeId, int totalHours);
    
    Task<ActionResponse<CourseHourType>> Edit(Guid id,Guid courseId,Guid hourTypeId, int totalHours);

    Task<ActionResponse> Delete(Guid id);

}