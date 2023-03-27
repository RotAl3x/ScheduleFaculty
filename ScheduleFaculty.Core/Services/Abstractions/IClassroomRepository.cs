using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;
public interface IClassroomRepository
{
    Task<ActionResponse<ClassroomResponse>> GetClassroom(Guid id);
    Task<ActionResponse<List<ClassroomResponse>>> GetAllClassrooms();

    Task<ActionResponse<ClassroomResponse>> CreateClassroom(string name, List<DaysOfWeek> freeDays);
    Task<ActionResponse<ClassroomResponse>> EditClassroom(Guid id, string name, List<DaysOfWeek> freeDays);
    Task<ActionResponse> DeleteClassroom(Guid id);
    
    Task SaveClassrooms();
}