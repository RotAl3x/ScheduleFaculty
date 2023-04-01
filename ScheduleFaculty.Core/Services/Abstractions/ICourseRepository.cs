using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface ICourseRepository
{
    Task<ActionResponse<Course>> GetCourseById(Guid id);
    Task<ActionResponse<List<Course>>> GetCoursesByProfessorId(Guid professorId);
    Task<ActionResponse<List<Course>>> GetCoursesByStudyProgramId(Guid studyProgramId);
    Task<ActionResponse<List<Course>>> GetAllCourses();

    Task<ActionResponse<Course>> CreateCourse(Guid studyProgramId, Guid professorId, string name, string abbreviation,
        int semester, bool isOptional);

    Task<ActionResponse<Course>> EditCourse(Course course);
    Task<ActionResponse> DeleteCourse(Guid id);

}