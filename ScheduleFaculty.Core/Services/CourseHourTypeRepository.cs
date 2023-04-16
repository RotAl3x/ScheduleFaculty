using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class CourseHourTypeRepository : ICourseHourTypeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CourseHourTypeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<ActionResponse<CourseHourType>> GetById(Guid id)
    {
        var response = new ActionResponse<CourseHourType>();
        var course = await _dbContext.CourseHourTypes.SingleOrDefaultAsync(c => c.Id == id);

        if (course is null)
        {
            response.AddError("Course type hour doesn't exist");
            return response;
        }

        response.Item = course;
        return response;
    }

    public async Task<ActionResponse<CourseHourType>> GetByCourseId(Guid courseId)
    {
        var response = new ActionResponse<CourseHourType>();
        var course = await _dbContext.CourseHourTypes.SingleOrDefaultAsync(c => c.CourseId == courseId);

        if (course is null)
        {
            response.AddError("Course type hour doesn't exist");
            return response;
        }

        response.Item = course;
        return response;
    }

    public async Task<ActionResponse<CourseHourType>> GetByHourType(Guid hourTypeId)
    {
        var response = new ActionResponse<CourseHourType>();
        var course = await _dbContext.CourseHourTypes.SingleOrDefaultAsync(c => c.HourTypeId == hourTypeId);

        if (course is null)
        {
            response.AddError("Course type hour doesn't exist");
            return response;
        }

        response.Item = course;
        return response;
    }

    public async Task<ActionResponse<CourseHourType>> Create(Guid courseId, Guid hourTypeId, int totalHours)
    {
        var response = new ActionResponse<CourseHourType>();

        var course = new CourseHourType { CourseId = courseId, HourTypeId = hourTypeId, TotalHours = totalHours };
        var dbCourseHourType = await _dbContext.CourseHourTypes.AddAsync(course);
        await _dbContext.SaveChangesAsync();

        response.Item = dbCourseHourType.Entity;
        return response;
    }

    public async Task<ActionResponse<CourseHourType>> Edit(Guid id, Guid courseId, Guid hourTypeId, int totalHours)
    {
        var response = new ActionResponse<CourseHourType>();

        var courseHourTypeToChange =
            await _dbContext.CourseHourTypes.SingleOrDefaultAsync(c => c.Id == id);

        if (courseHourTypeToChange is null)
        {
            response.AddError("Course type hour doesn't exists");
            return response;
        }

        courseHourTypeToChange.CourseId = courseId;
        courseHourTypeToChange.HourTypeId = hourTypeId;
        courseHourTypeToChange.TotalHours = totalHours;
        await _dbContext.SaveChangesAsync();

        response.Item = courseHourTypeToChange;
        return response;
    }

    public async Task<ActionResponse> Delete(Guid id)
    {
        var response = new ActionResponse();

        var course = await _dbContext.CourseHourTypes.SingleOrDefaultAsync(c => c.Id == id);
        if (course is null)
        {
            response.AddError("Course type hour doesn't exist");
            return response;
        }

        _dbContext.CourseHourTypes.Remove(course);
        await _dbContext.SaveChangesAsync();

        return response;
    }
}