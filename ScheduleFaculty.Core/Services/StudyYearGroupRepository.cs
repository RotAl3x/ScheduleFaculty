using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using ScheduleFaculty.Core.Database;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Migrations;
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
            response.AddError("Student program doesn't have groups");
            return response;
        }

        response.Item = studyYearGroup;
        return response;
    }

    public async Task<ActionResponse<NumberOfGroupsOfYear>> GetStudyYearNumberOfGroups(Guid studyProgramId)
    {
        var response = new ActionResponse<NumberOfGroupsOfYear>();
        var numberOfGroups =
            await _dbContext.NumberOfGroupsOfYears.SingleOrDefaultAsync(n => n.StudyProgramYearId == studyProgramId);
        if (numberOfGroups is null)
        {
            response.AddError("Student program doesn't have groups");
            return response;
        }

        response.Item = numberOfGroups;
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

        var hasMaxGroups =
            await _dbContext.NumberOfGroupsOfYears.SingleOrDefaultAsync(n => n.StudyProgramYearId == studyProgramId);
        if (studyProgramHasGroups)
        {
            response.AddError("Study program have max groups");
            return response;
        }

        var maxGroups = new NumberOfGroupsOfYear
        {
            NumberOfSemiGroups = numberOfSemiGroups,
            HowManySemiGroupsAreInAGroup = howManySemiGroupsAreInAGroup,
            StudyProgramYearId = studyProgramId
        };

        await _dbContext.NumberOfGroupsOfYears.AddAsync(maxGroups);

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
        var hasMaxGroups =
            await _dbContext.NumberOfGroupsOfYears.SingleOrDefaultAsync(n => n.StudyProgramYearId == studyProgramId);

        var studyYearGroups = await _dbContext.StudyYearGroups
            .Where(s => s.StudyProgramYearId == studyProgramId).OrderBy(s => s.SemiGroup).ToListAsync();
        if (studyYearGroups.Count==0)
        {
            response.AddError("Student program doesn't have groups");
            return response;
        }

        if (hasMaxGroups is null)
        {
            response.AddError("Student program doesn't have groups");
            return response;
        }

        var semiGroupToAdd = new List<StudyYearGroup>();
        
        if (hasMaxGroups.HowManySemiGroupsAreInAGroup != howManySemiGroupsAreInAGroup)
        {
            for (var i = 0; i < hasMaxGroups.NumberOfSemiGroups; i++)
            {
                studyYearGroups[i].Name =
                    i / howManySemiGroupsAreInAGroup + 1 + "." + (i % howManySemiGroupsAreInAGroup + 1);
                studyYearGroups[i].Group = i / howManySemiGroupsAreInAGroup + 1;
            }
        }
        
        if (hasMaxGroups.NumberOfSemiGroups < numberOfSemiGroups)
        {
            for (var i = hasMaxGroups.NumberOfSemiGroups; i < numberOfSemiGroups; i++)
            {
                semiGroupToAdd.Add(new StudyYearGroup
                {
                    SemiGroup = i+1 ,
                    Group = i / howManySemiGroupsAreInAGroup + 1,
                    Name = i / howManySemiGroupsAreInAGroup + 1 + "." + (i % howManySemiGroupsAreInAGroup + 1),
                    StudyProgramYearId = studyProgramId
                });
            }
            await _dbContext.StudyYearGroups.AddRangeAsync(semiGroupToAdd);
            studyYearGroups.AddRange(semiGroupToAdd);
        }

        if (hasMaxGroups.NumberOfSemiGroups > numberOfSemiGroups)
        {
            _dbContext.StudyYearGroups.RemoveRange(studyYearGroups.GetRange(numberOfSemiGroups,hasMaxGroups.NumberOfSemiGroups- numberOfSemiGroups));
            studyYearGroups.RemoveRange(numberOfSemiGroups,hasMaxGroups.NumberOfSemiGroups- numberOfSemiGroups);
        }

        hasMaxGroups.NumberOfSemiGroups = numberOfSemiGroups;
        hasMaxGroups.HowManySemiGroupsAreInAGroup = howManySemiGroupsAreInAGroup;
        
        await _dbContext.SaveChangesAsync();

        response.Item = studyYearGroups;
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