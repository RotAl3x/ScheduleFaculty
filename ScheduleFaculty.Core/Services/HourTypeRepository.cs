using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class HourTypeRepository: IHourTypeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public HourTypeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ActionResponse<HourType>> GetHourType(Guid id)
    {
        var response = new ActionResponse<HourType>();
        var hourType = await _dbContext.HourTypes.SingleOrDefaultAsync(s => s.Id == id);

        if (hourType is null)
        {
            response.AddError("Type hour doesn't exist");
            return response;
        }

        response.Item = hourType;
        return response;
    }

    public async Task<ActionResponse<List<HourType>>> GetAllHourType()
    {
        var response = new ActionResponse<List<HourType>>();
        var hourType = await _dbContext.HourTypes.ToListAsync();

        response.Item = hourType;
        return response;
    }

    public async Task<ActionResponse<HourType>> CreateHourType(string name, int semiGroupsPerHour, bool needAllSemiGroup)
    {
        var response = new ActionResponse<HourType>();
        var nameExist = await _dbContext.HourTypes.AnyAsync(h => h.Name == name);

        if (nameExist)
        {
            response.AddError("Type hour with the same name already exists");
            return response;
        }

        var hourType = new HourType()
        {
            Name = name,
            SemiGroupsPerHour = semiGroupsPerHour,
            NeedAllSemiGroups = needAllSemiGroup,
        };

        var dbHourType = await _dbContext.HourTypes.AddAsync(hourType);
        await _dbContext.SaveChangesAsync();
        response.Item = dbHourType.Entity;
        return response;
    }

    public async Task<ActionResponse<HourType>> EditHourType(HourType hourType)
    {
        var response = new ActionResponse<HourType>();
        var hour = await _dbContext.HourTypes.SingleOrDefaultAsync(h => h.Id == hourType.Id);
        if (hour is null)
        {
            response.AddError("Type hour doesn't exist");
            return response;
        }

        hour.Name = hourType.Name;
        hour.SemiGroupsPerHour = hourType.SemiGroupsPerHour;
        hour.NeedAllSemiGroups = hourType.NeedAllSemiGroups;
        await _dbContext.SaveChangesAsync();
        response.Item = hour;
        return response;
    }

    public async Task<ActionResponse> DeleteHourType(Guid id)
    {
        var response = new ActionResponse();

        var hourTypeDelete = await _dbContext.HourTypes.SingleOrDefaultAsync(h => h.Id == id);

        if (hourTypeDelete is null)
        {
            response.AddError("Type hour doesn't exist");
            return response;
        }

        _dbContext.HourTypes.Remove(hourTypeDelete);
        await _dbContext.SaveChangesAsync();

        return response;
    }
}