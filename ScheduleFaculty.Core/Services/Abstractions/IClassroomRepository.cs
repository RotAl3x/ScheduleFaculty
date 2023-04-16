using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;
public interface IClassroomRepository
{
    Task<ActionResponse<Classroom>> GetClassroom(Guid id);
    Task<ActionResponse<List<Classroom>>> GetAllClassrooms();

    Task<ActionResponse<Classroom>> CreateClassroom(string name, List<DayOfWeek> freeDays);
    Task<ActionResponse<Classroom>> EditClassroom(Guid id, string name, List<DayOfWeek> freeDays);
    Task<ActionResponse> DeleteClassroom(Guid id);
    
    Task SaveClassrooms();
}