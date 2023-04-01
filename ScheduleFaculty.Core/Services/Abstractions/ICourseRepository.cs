using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface ICourseRepository
{
    Task<ActionResponse<Course>> GetCourseById(Guid id);
    Task<ActionResponse<List<Course>>> GetCoursesByProfessorId(string professorId);
    Task<ActionResponse<List<Course>>> GetCoursesByStudyProgramId(Guid studyProgramId);
    Task<ActionResponse<List<Course>>> GetAllCourses();

    Task<ActionResponse<Course>> CreateCourse(Guid studyProgramId, string professorId, string name, string abbreviation,
        int semester, bool isOptional);

    Task<ActionResponse<Course>> EditCourse(Guid id,Guid studyProgramId, string professorId, string name,
        string abbreviation, int semester,
        bool isOptional);
    Task<ActionResponse> DeleteCourse(Guid id);

}