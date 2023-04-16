using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CourseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ActionResponse<Course>> GetCourseById(Guid id)
    {
        var response = new ActionResponse<Course>();
        var course = await _dbContext.Courses.SingleOrDefaultAsync(c => c.Id == id);

        if (course is null)
        {
            response.AddError("Course doesn't exist");
            return response;
        }

        response.Item = course;
        return response;
    }

    public async Task<ActionResponse<List<Course>>> GetCoursesByProfessorId(string professorId)
    {
        var response = new ActionResponse<List<Course>>();
        var courses = await _dbContext.Courses.Where(c => c.ProfessorUserId == professorId).ToListAsync();

        response.Item = courses;
        return response;
    }

    public async Task<ActionResponse<List<Course>>> GetCoursesByStudyProgramId(Guid studyProgramId)
    {
        var response = new ActionResponse<List<Course>>();
        var courses = await _dbContext.Courses.Where(c => c.StudyProgramYearId == studyProgramId).ToListAsync();

        response.Item = courses;
        return response;
    }

    public async Task<ActionResponse<List<Course>>> GetAllCourses()
    {
        var response = new ActionResponse<List<Course>>();
        var courses = await _dbContext.Courses.ToListAsync();

        response.Item = courses;
        return response;
    }

    public async Task<ActionResponse<Course>> CreateCourse(Guid studyProgramId, string professorId, string name,
        string abbreviation, int semester,
        bool isOptional)
    {
        var response = new ActionResponse<Course>();
        if (semester > 2)
        {
            response.AddError("Semesters are 1 or 2");
            return response;
        }

        var course = new Course
        {
            StudyProgramYearId = studyProgramId,
            ProfessorUserId = professorId,
            Name = name,
            Abbreviation = abbreviation,
            Semester = semester,
            IsOptional = isOptional,
        };
        var dbCourse = await _dbContext.Courses.AddAsync(course);
        await _dbContext.SaveChangesAsync();

        response.Item = dbCourse.Entity;
        return response;
    }

    public async Task<ActionResponse<Course>> EditCourse(Guid id,Guid studyProgramId, string professorId, string name,
        string abbreviation, int semester,
        bool isOptional)
    {
        var response = new ActionResponse<Course>();

        var courseToEdit = await _dbContext.Courses.SingleOrDefaultAsync(c => c.Id == id);

        if (courseToEdit is null)
        {
            response.AddError("Course doesn't exist");
            return response;
        }
        if (semester > 2)
        {
            response.AddError("Semesters are 1 or 2");
            return response;
        }

        courseToEdit.StudyProgramYearId = studyProgramId;
        courseToEdit.ProfessorUserId = professorId;
        courseToEdit.Name = name;
        courseToEdit.Abbreviation = abbreviation;
        courseToEdit.Semester = semester;
        courseToEdit.IsOptional = isOptional;
        await _dbContext.SaveChangesAsync();
        response.Item = courseToEdit;
        return response;
    }

    public async Task<ActionResponse> DeleteCourse(Guid id)
    {
        var response = new ActionResponse<Course>();

        var courseToDelete = await _dbContext.Courses.SingleOrDefaultAsync(c => c.Id == id);

        if (courseToDelete is null)
        {
            response.AddError("Course doesn't exist");
            return response;
        }

        _dbContext.Courses.Remove(courseToDelete);
        await _dbContext.SaveChangesAsync();

        return response;
    }
}