using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class ClassroomRepository : IClassroomRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ClassroomRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ActionResponse<ClassroomResponse>> GetClassroom(Guid id)
    {
        var response = new ActionResponse<ClassroomResponse>();

        var freeDaysClassrooms = await _dbContext.FreeDaysClassrooms
            .Where(f => f.ClassroomId == id).Select(f=>f.DaysOfWeek).ToListAsync();

        var classroom = await _dbContext.Classrooms.SingleOrDefaultAsync(c => c.Id == id);

        if (classroom is null)
        {
            response.AddError("Classroom doesn't exists");
            return response;
        }

        response.Item.Id = classroom.Id;
        response.Item.Name = classroom.Name;
        response.Item.DaysOfWeeks = freeDaysClassrooms; 

        return response;
    }

    public async Task<ActionResponse<List<ClassroomResponse>>> GetAllClassrooms()
    {
        var response = new ActionResponse<List<ClassroomResponse>>();

        var classroomsWithFreeDays = new List<ClassroomResponse>();

        var classrooms = await _dbContext.Classrooms.ToListAsync();
        var freeDaysClassrooms = _dbContext.FreeDaysClassrooms;
        
        var i = 0;
        foreach (var c in classrooms)
        {
            classroomsWithFreeDays[i].Id = c.Id;
            classroomsWithFreeDays[i].Name = c.Name;
            classroomsWithFreeDays[i].DaysOfWeeks = await freeDaysClassrooms
                .Where(f => f.ClassroomId == c.Id)
                .Select(f => f.DaysOfWeek).ToListAsync();
            i++;
        }


        response.Item = classroomsWithFreeDays;
        
        return response;
    }

    public async Task<ActionResponse<ClassroomResponse>> CreateClassroom(string name, List<DaysOfWeek> freeDays)
    {
        var response = new ActionResponse<ClassroomResponse>();

        var classroom = new Classroom
        {
                Name = name
        };

        var dbClassroom = await _dbContext.AddAsync(classroom);
        await _dbContext.SaveChangesAsync();
        
        freeDays.ForEach(f =>
        {
            
        });
    }

    public Task<ActionResponse<ClassroomResponse>> EditClassroom(Guid id, string name, List<DaysOfWeek> freeDays)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse> DeleteClassroom(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task SaveClassrooms()
    {
        throw new NotImplementedException();
    }
}