using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class StatusRepository : IStatusRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StatusRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<ActionResponse<Status>> GetStatus(Guid id)
    {
        var response = new ActionResponse<Status>();
        var status = await _dbContext.Statuses.SingleOrDefaultAsync(s => s.Id == id);

        if (status is null)
        {
            response.AddError("Status doesn't exist");
            return response;
        }

        response.Item = status;
        return response;
    }

    public async Task<ActionResponse<Status>> GetActiveStatus()
    {
        var response = new ActionResponse<Status>();
        var status = await _dbContext.Statuses.SingleOrDefaultAsync(s => s.IsActive == true);

        if (status is null)
        {
            response.AddError("Status doesn't exist");
            return response;
        }

        response.Item = status;
        return response;
    }

    public async Task<ActionResponse<List<Status>>> GetAllStatuses()
    {
        var response = new ActionResponse<List<Status>>();
        var status = await _dbContext.Statuses.ToListAsync();

        response.Item = status;
        return response;
    }

    public async Task<ActionResponse<Status>> CreateStatus(string name)
    {
        var response = new ActionResponse<Status>();
        var statusExist = await _dbContext.Statuses.AnyAsync(s => s.Name == name);

        if (statusExist)
        {
            response.AddError("Status with the same name already exists");
            return response;
        }

        var status = new Status()
        {
            Name = name,
            Semester = 1,
            IsActive = false,
        };
        var dbStatus = await _dbContext.Statuses.AddAsync(status);
        await _dbContext.SaveChangesAsync();
        response.Item = dbStatus.Entity;
        return response;
    }

    public async Task<ActionResponse<Status>> EditStatus(Guid id, string name, bool isActive, int semester)
    {
        var response = new ActionResponse<Status>();
        var statusEntity = _dbContext.Statuses;
        var allStatuses = await statusEntity.ToListAsync();
        var status = await statusEntity.SingleOrDefaultAsync(s => s.Id == id);

        if (status is null)
        {
            response.AddError("Status doesn't exist");
            return response;
        }

        if (semester > 2)
        {
            response.AddError("Semesters are only 1 or 2");
            return response;
        }

        if (isActive)
        {
            foreach (var statusToFalse in allStatuses)
            {
                statusToFalse.IsActive = false;
            }
        }

        status.IsActive = isActive;
        status.Name = name;
        status.Semester = semester;
        await _dbContext.SaveChangesAsync();

        response.Item = status;
        return response;
    }

    public async Task<ActionResponse> DeleteStatus(Guid id)
    {
        var response = new ActionResponse();
        var statusToDelete = await _dbContext.Statuses.SingleOrDefaultAsync(s => s.Id == id);

        if (statusToDelete is null)
        {
            response.AddError("Status doesn't exists");
            return response;
        }

        _dbContext.Statuses.Remove(statusToDelete);
        await _dbContext.SaveChangesAsync();
        
        return response;
    }
}