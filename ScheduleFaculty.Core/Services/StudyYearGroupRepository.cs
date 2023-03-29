using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;

namespace ScheduleFaculty.Core.Services;

public class StudyYearGroupRepository : IStudyYearGroupRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StudyYearGroupRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<ActionResponse<List<StudyYearGroup>>> GetStudyYearGroup(Guid studyProgramId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<List<StudyYearGroup>>>> GetAllStudyYearGroups()
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<TotalGroups>> GetStudyYearNumberOfGroups(Guid studyProgramId)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse<List<TotalGroups>>> GetAllStudyYearNumberOfGroups()
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResponse<List<StudyYearGroup>>> CreateStudyYearGroup(Guid studyProgramId,
        int numberOfSemiGroups, int howManySemiGroupsAreInAGroup)
    {
        var response = new ActionResponse<List<StudyYearGroup>>();

        var studyProgramHasGroups =
            await _dbContext.StudyYearGroups.AnyAsync(s => s.StudyProgramYearId == studyProgramId);

        if (studyProgramHasGroups)
        {
            response.AddError("Study program have groups");
            return response;
        }

        var dbGroups = new List<StudyYearGroup>();

        for (var i = 0; i < numberOfSemiGroups; i++)
        {
            var semiGroup = new StudyYearGroup
            {
                SemiGroup = i+1,
                Group = i / howManySemiGroupsAreInAGroup + 1,
                Name = i / howManySemiGroupsAreInAGroup + 1 + "." + (i % howManySemiGroupsAreInAGroup + 1),
                StudyProgramYearId = studyProgramId
            };
            var dbStudyYearGroup = await _dbContext.StudyYearGroups.AddAsync(semiGroup);
            dbGroups.Add(dbStudyYearGroup.Entity);
        }

        await _dbContext.SaveChangesAsync();
        response.Item = dbGroups;
        return response;
    }

    public Task<ActionResponse<List<StudyYearGroup>>> EditStudyYearGroup(Guid studyProgramId, int numberOfSemiGroups,
        int howManySemiGroupsAreInAGroup)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResponse> DeleteStudyYearGroup(Guid studyProgramId)
    {
        throw new NotImplementedException();
    }

    public Task SaveStudyYearGroup()
    {
        throw new NotImplementedException();
    }
}