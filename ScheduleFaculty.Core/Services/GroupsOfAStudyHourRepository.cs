using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class GroupsOfAStudyHourRepository:IGroupsOfAStudyHourRepository
{
    private readonly ApplicationDbContext _dbContext;

    public GroupsOfAStudyHourRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    public async Task<ActionResponse<List<StudyYearGroup>>> GetByHourStudyId(Guid id)
    {
        var response = new ActionResponse<List<StudyYearGroup>>();
        var groupsHourStudyOfAYear =
            await _dbContext.GroupsOfAStudyHour.Where(g => g.HourStudyOfAYearId == id)
                .Include(g=>g.StudyYearGroup)
                .Select(g=>g.StudyYearGroup).ToListAsync();
        response.Item = groupsHourStudyOfAYear;
        return response;
    }
}