using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services.Abstractions;

public interface IHourTypeRepository
{
    Task<ActionResponse<HourType>> GetHourType(Guid id);

    Task<ActionResponse<List<HourType>>> GetAllHourType();
    
    Task<ActionResponse<HourType>> CreateHourType(string name, int semiGroupsPerHour, bool needAllSemiGroup);
    
    Task<ActionResponse<HourType>> EditHourType(HourType hourType);
    
    Task<ActionResponse> DeleteHourType(Guid id);
}