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

    public async Task<ActionResponse<Classroom>> GetClassroom(Guid id)
    {
        var response = new ActionResponse<Classroom>();
        var classroom = await _dbContext.Classrooms.SingleOrDefaultAsync(c => c.Id == id);

        if (classroom == null)
        {
            response.AddError("Classroom doesn't exist");
            return response;
        }

        response.Item = classroom;
        return response;
    }

    public async Task<ActionResponse<List<Classroom>>> GetAllClassrooms()
    {
        var response = new ActionResponse<List<Classroom>>();
        var classrooms = await _dbContext.Classrooms.ToListAsync();

        response.Item = classrooms;
        return response;
    }

    public async Task<ActionResponse<Classroom>> CreateClassroom(string name, List<DayOfWeek> freeDays, string? MACAddress)
    {
        var response = new ActionResponse<Classroom>();

        var nameExists = await _dbContext.Classrooms.AnyAsync(c => c.Name == name);

        if (nameExists)
        {
            response.AddError("Classroom with the same name already exists");
            return response;
        }

        var classroom = new Classroom { Name = name, DaysOfWeek = freeDays, MACAddress = MACAddress};
        var dbClassroom = await _dbContext.Classrooms.AddAsync(classroom);
        await _dbContext.SaveChangesAsync();

        response.Item = dbClassroom.Entity;
        return response;
    }

    public async Task<ActionResponse<Classroom>> EditClassroom(Guid id, string name, List<DayOfWeek> freeDays,string? MACAddress)
    {
        var response = new ActionResponse<Classroom>();

        var classroomToChange = await _dbContext.Classrooms.SingleOrDefaultAsync(c => c.Id == id);

        if (classroomToChange is null)
        {
            response.AddError("Classroom doesn't exist");
            return response;
        }

        classroomToChange.Name = name;
        classroomToChange.DaysOfWeek = freeDays;
        classroomToChange.MACAddress = MACAddress;
        await _dbContext.SaveChangesAsync();
        response.Item = classroomToChange;
        return response;
    }

    public async Task<ActionResponse> DeleteClassroom(Guid id)
    {
        var response = new ActionResponse();

        var classroomToDelete = await _dbContext.Classrooms.SingleOrDefaultAsync(c => c.Id == id);
        
        if (classroomToDelete is null)
        {
            response.AddError("Classroom doesn't exist");
            return response;
        }

        _dbContext.Classrooms.Remove(classroomToDelete);
        await _dbContext.SaveChangesAsync();

        return response;
    }

    public async Task SaveClassrooms()
    {
        await _dbContext.SaveChangesAsync();
    }
}