using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface IFreeDayClassroomRepository
{
    Task<ActionResponse<FreeDaysClassroom>> GetFreeDaysClassroom(Guid id);

    Task<ActionResponse<FreeDaysClassroom>> GetAllFreeDaysClassroom();

    Task<ActionResponse<FreeDaysClassroom>> AddFreeDayClassroom(Guid weekDayId, Guid classroomId);


    Task SaveClassrooms();
}