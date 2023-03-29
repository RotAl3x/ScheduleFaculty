using Microsoft.CodeAnalysis.VisualBasic.Syntax;
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

    public async Task<ActionResponse<List<StudyYearGroup>>> GetStudyYearGroup(Guid studyProgramId)
    {
        var response = new ActionResponse<List<StudyYearGroup>>();
        var studyYearGroup = await _dbContext.StudyYearGroups.Where(s => s.StudyProgramYearId == studyProgramId)
            .ToListAsync();
        if (studyYearGroup.Count == 0)
        {
            response.AddError("Student program doesn't exist");
            return response;
        }

        response.Item = studyYearGroup;
        return response;
    }

    public async Task<ActionResponse<TotalGroups>> GetStudyYearNumberOfGroups(Guid studyProgramId)
    {
        var response = new ActionResponse<TotalGroups>();
        var studyYearNumberOfGroups = await _dbContext.StudyYearGroups.OrderByDescending(s => s.SemiGroup)
            .FirstOrDefaultAsync(s => s.StudyProgramYearId == studyProgramId);
        if (studyYearNumberOfGroups is null)
        {
            response.AddError("Student program doesn't exist");
            return response;
        }

        var totalGroup = new TotalGroups
        {
            StudyProgramId = studyProgramId,
            NumberOfSemiGroups = studyYearNumberOfGroups.SemiGroup,
            HowManySemiGroupsAreInAGroup = (studyYearNumberOfGroups.SemiGroup - 1) / studyYearNumberOfGroups.Group + 1
        };

        response.Item = totalGroup;
        return response;
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

        var semiGroup = new List<StudyYearGroup>();

        for (var i = 0; i < numberOfSemiGroups; i++)
        {
            semiGroup.Add(new StudyYearGroup
            {
                SemiGroup = i + 1,
                Group = i / howManySemiGroupsAreInAGroup + 1,
                Name = i / howManySemiGroupsAreInAGroup + 1 + "." + (i % howManySemiGroupsAreInAGroup + 1),
                StudyProgramYearId = studyProgramId
            });
        }

        await _dbContext.StudyYearGroups.AddRangeAsync(semiGroup);
        await _dbContext.SaveChangesAsync();
        response.Item = semiGroup;
        return response;
    }

    public async Task<ActionResponse<List<StudyYearGroup>>> EditStudyYearGroup(Guid studyProgramId,
        int numberOfSemiGroups,
        int howManySemiGroupsAreInAGroup)
    {
        var response = new ActionResponse<List<StudyYearGroup>>();
        var studyYearNumberOfGroupsEntity = _dbContext.StudyYearGroups.OrderByDescending(s => s.SemiGroup);
        var studyYearNumberOfGroups =
            await studyYearNumberOfGroupsEntity.FirstOrDefaultAsync(s => s.StudyProgramYearId == studyProgramId);
        var groupsToList = await studyYearNumberOfGroupsEntity.ToListAsync();
        if (studyYearNumberOfGroups is null)
        {
            response.AddError("Student program doesn't exist");
            return response;
        }


        var semiGroup = new List<StudyYearGroup>();
        if (howManySemiGroupsAreInAGroup != ((studyYearNumberOfGroups.SemiGroup - 1) / studyYearNumberOfGroups.Group + 1))
        {
            for (var i = studyYearNumberOfGroups.SemiGroup; i > 0; i--)
            {
                groupsToList[i].Name =
                    i / howManySemiGroupsAreInAGroup + 1 + "." + (i % howManySemiGroupsAreInAGroup + 1);
                groupsToList[i].Group = i / howManySemiGroupsAreInAGroup + 1;
            }

            _dbContext.StudyYearGroups.UpdateRange(groupsToList);
        }

        if (studyYearNumberOfGroups.SemiGroup < numberOfSemiGroups)
        {
            for (var i = studyYearNumberOfGroups.SemiGroup; i < numberOfSemiGroups; i++)
            {
                semiGroup.Add(new StudyYearGroup
                {
                    SemiGroup = i + 1,
                    Group = i / howManySemiGroupsAreInAGroup + 1,
                    Name = i / howManySemiGroupsAreInAGroup + 1 + "." + (i % howManySemiGroupsAreInAGroup + 1),
                    StudyProgramYearId = studyProgramId
                });
            }

            await _dbContext.StudyYearGroups.AddRangeAsync(semiGroup);
            groupsToList.AddRange(semiGroup);
        }

        if (studyYearNumberOfGroups.SemiGroup > numberOfSemiGroups)
        {
            var rangeToRemove = groupsToList.GetRange(numberOfSemiGroups + 1,
                groupsToList.Count);
            _dbContext.StudyYearGroups.RemoveRange(rangeToRemove);
            groupsToList.RemoveRange(numberOfSemiGroups + 1,
                groupsToList.Count);
        }

        response.Item = groupsToList;
        await _dbContext.SaveChangesAsync();


        return response;
    }

    public async Task<ActionResponse> DeleteStudyYearGroup(Guid studyProgramId)
    {
        var response = new ActionResponse();

        var groupsToDelete = await _dbContext.StudyYearGroups.Where(s => s.StudyProgramYearId == studyProgramId)
            .ToListAsync();
        if (groupsToDelete.Count == 0)
        {
            response.AddError("Student program doesn't exist");
            return response;
        }

        _dbContext.StudyYearGroups.RemoveRange(groupsToDelete);
        await _dbContext.SaveChangesAsync();
        return response;
    }

    public async Task SaveStudyYearGroup()
    {
        await _dbContext.SaveChangesAsync();
    }
}