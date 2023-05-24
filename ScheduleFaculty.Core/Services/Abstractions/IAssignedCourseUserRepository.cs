using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface IAssignedCourseUserRepository
{
    Task<ActionResponse<List<AssignedCourseUser>>> GetAllAssignedCourseUser();
    Task<ActionResponse<AssignedCourseUser>> GetById(Guid id);
    Task<ActionResponse<List<AssignedCourseUser>>> GetAssignedCourseUserByCourseId(Guid courseId);
    Task<ActionResponse<List<AssignedCourseUser>>> GetAssignedCourseUsersByProfessorId(string professorId);
    Task<ActionResponse<AssignedCourseUser>> CreateAssignedCourseUser(Guid courseId, string professorId);
    
    Task<ActionResponse<AssignedCourseUser>> EditAssignedCourseUser(Guid id,Guid courseId, string professorId);

    Task<ActionResponse> DeleteAssignedCourseUser(Guid id);
}