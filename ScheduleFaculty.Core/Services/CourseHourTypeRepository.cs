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

    public async Task<ActionResponse<List<CourseHourType>>> GetByCourseId(Guid courseId)
    {
        var response = new ActionResponse<List<CourseHourType>>();
        var course = await _dbContext.CourseHourTypes.Where(c => c.CourseId == courseId)
            .Include(c=>c.HourType)
            .ToListAsync();

        if (course.Count == 0)
        {
            response.AddError("Course type hour doesn't exist");
            return response;
        }

        response.Item = course;
        return response;
    }

    public async Task<ActionResponse<List<CourseHourType>>> GetByHourType(Guid hourTypeId)
    {
        var response = new ActionResponse<List<CourseHourType>>();
        var course = await _dbContext.CourseHourTypes.Where(c => c.HourTypeId == hourTypeId).ToListAsync();

        if (course.Count == 0)
        {
            response.AddError("Course type hour doesn't exist");
            return response;
        }

        response.Item = course;
        return response;
    }

    public async Task<ActionResponse<List<HourType>>> GetHourTypesByCourseId(Guid courseId)
    {
        var response = new ActionResponse<List<HourType>>();
        var hourTypes = await _dbContext.CourseHourTypes.Where(c => c.CourseId == courseId).Include(c => c.HourType)
            .Select(c => c.HourType).ToListAsync();

        if (hourTypes.Count == 0)
        {
            response.AddError("Course type hour doesn't exist");
            return response;
        }

        response.Item = hourTypes;
        return response;
    }

    public async Task<ActionResponse<CourseHourType>> Create(Guid courseId, Guid hourTypeId)
    {
        var response = new ActionResponse<CourseHourType>();

        var course = new CourseHourType { CourseId = courseId, HourTypeId = hourTypeId };
        var dbCourseHourType = await _dbContext.CourseHourTypes.AddAsync(course);
        await _dbContext.SaveChangesAsync();

        response.Item = dbCourseHourType.Entity;
        return response;
    }

    public async Task<ActionResponse<CourseHourType>> Edit(Guid id, Guid courseId, Guid hourTypeId)
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
        await _dbContext.SaveChangesAsync();

        response.Item = courseHourTypeToChange;
        return response;
    }

    public async Task<ActionResponse<List<HourType>>> EditHourTypes(Guid courseId, List<Guid> hourTypeIds)
    {
        var response = new ActionResponse<List<HourType>>();
        var courseHourTypes= await _dbContext.CourseHourTypes.Where(c => c.CourseId == courseId)
            .Include(c => c.HourType)
            .ToListAsync();

        if (courseHourTypes.Count == 0)
        {
            response.AddError("Course type hour doesn't exist");
            return response;
        }

        var courseHourTypeId = new List<Guid>();

        foreach (var courseHourType in courseHourTypes)
        {
            courseHourTypeId.Add(courseHourType.HourTypeId);
        }

        var hourTypeToAdd=hourTypeIds.Except(courseHourTypeId);

        if (hourTypeToAdd.Any())
        {
            foreach (var hourId in hourTypeToAdd)
            {
                var courseToAdd = new CourseHourType { CourseId = courseId, HourTypeId = hourId };
                await _dbContext.CourseHourTypes.AddAsync(courseToAdd);
            }
        }
        
        foreach (var course in courseHourTypes)
        {
            if (!hourTypeIds.Contains(course.HourTypeId))
            {
                _dbContext.CourseHourTypes.Remove(course);
            }
        }
        
        await _dbContext.SaveChangesAsync();
        
        response.Item = await _dbContext.CourseHourTypes.Where(c => c.CourseId == courseId)
            .Include(c => c.HourType)
            .Select(c=>c.HourType)
            .ToListAsync();
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